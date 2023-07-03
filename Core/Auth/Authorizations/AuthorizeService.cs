using Core.Auth.Sessions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using DotNetCore.CAP;

namespace Core.Auth.Authorizations;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;
    private readonly ICapPublisher _eventBus;

    public AuthorizeService(IAuthorizeDataProvider authorizeDataProvider, ICapPublisher eventBus)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _eventBus = eventBus;
    }

    public async Task<Guid> AuthorizePermissionAsync(string token, IEnumerable<string> actions, string serviceSecret, string? userAgent, string ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await ValidateSession(token, cancellationToken);
        ValidateUser(session);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        ValidateSessionScope(session, userRoles);
        var service = await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        if (InvalidPermission())
        {
            throw new ForbiddenException();
        }

        var _ = Task.Run(() =>
        {
            _eventBus.PublishAsync(AuthorizedEvent.EventName, new AuthorizedEvent
            {
                Agent = userAgent,
                IpAddress = ipAddress,
                SessionId = session.Id,
                UserId = session.User.Id
            }, cancellationToken: cancellationToken);
        }, cancellationToken);

        return session.User.Id;

        bool InvalidPermission() => userRoles.SelectMany(role => role.Permissions)
            .All(permission => actions.All(action => permission.Name != $"{service.Name}_{action.ToLower()}"));
    }

    public async Task<Guid> AuthorizeRoleAsync(string token, IEnumerable<string> roles, string serviceSecret, string? userAgent, string ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await ValidateSession(token, cancellationToken);
        ValidateUser(session);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        ValidateSessionScope(session, userRoles);
        await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        if (InvalidRole())
        {
            throw new ForbiddenException();
        }

        var _ = Task.Run(() =>
        {
            _eventBus.PublishAsync(AuthorizedEvent.EventName, new AuthorizedEvent
            {
                Agent = userAgent,
                IpAddress = ipAddress,
                SessionId = session.Id,
                UserId = session.User.Id
            }, cancellationToken: cancellationToken);
        }, cancellationToken);

        return session.User.Id;
        bool InvalidRole() => userRoles.All(role => roles.All(r => role.Name != r.ToLower()));
    }

    private async Task<Session> ValidateSession(string token, CancellationToken cancellationToken)
    {
        var session = await _authorizeDataProvider.FindSessionAsync(token, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (SessionExpired())
        {
            throw new SessionExpiredException();
        }

        return session;

        bool SessionExpired() => DateTime.UtcNow >= session.ExpirationDateUtc;
    }

    private void ValidateUser(Session session)
    {
        if (UserIsNotActive())
        {
            throw new ForbiddenException();
        }

        bool UserIsNotActive() => session.User.Status != UserStatus.Active;
    }

    private void ValidateSessionScope(Session session, IEnumerable<Role> userRoles)
    {
        if (UserHasNotScopeRole())
        {
            throw new ForbiddenException();
        }

        bool UserHasNotScopeRole() => !userRoles.Any(role => session.Scope.Roles.Any(r => r.Id == role.Id));
    }

    private async Task<Service> ValidateRequestedService(string serviceSecret, Scope sessionScope, CancellationToken cancellationToken)
    {
        var service = await _authorizeDataProvider.RequestedServiceAsync(serviceSecret, cancellationToken);
        if (service is not null && !InvalidSessionScopeService()) return service;
        if (service is null || service.Name != EntityBaseValues.KunderaServiceName)
        {
            throw new ForbiddenException();
        }

        return service;
        bool InvalidSessionScopeService() => sessionScope.Services.All(s => s.Id != service.Id);
    }
}