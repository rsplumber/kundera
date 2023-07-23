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

    public async Task<(AuthorizeResponse?, UnAuthorizeResponse?)> AuthorizePermissionAsync(string token, IEnumerable<string> actions, string serviceSecret, string? userAgent, string ipAddress, CancellationToken cancellationToken = default)
    {
        var (session, sessionUnAuthorized) = await ValidateSession(token, cancellationToken);
        if (session is null)
        {
            return (null, sessionUnAuthorized);
        }

        var validateUser = ValidateUser(session);
        if (!validateUser) return (null, UnAuthorizeResponse.Forbidden);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        var validateSessionScope = ValidateSessionScope(session, userRoles);
        if (!validateSessionScope) return (null, UnAuthorizeResponse.Forbidden);
        var service = await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        if (service is null) return (null, UnAuthorizeResponse.Forbidden);
        if (InvalidPermission()) return (null, UnAuthorizeResponse.Forbidden);

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

        return (new AuthorizeResponse(session.User.Id)
        {
            ScopeId = session.Scope.Id,
            ServiceId = service.Id
        }, null);

        bool InvalidPermission() => userRoles.SelectMany(role => role.Permissions)
            .All(permission => actions.All(action => permission.Name != $"{service.Name}:{action}".ToLower()));
    }

    public async Task<(AuthorizeResponse?, UnAuthorizeResponse?)> AuthorizeRoleAsync(string token, IEnumerable<string> roles, string serviceSecret, string? userAgent, string ipAddress, CancellationToken cancellationToken = default)
    {
        var (session, sessionUnAuthorized) = await ValidateSession(token, cancellationToken);
        if (session is null)
        {
            return (null, sessionUnAuthorized);
        }

        var validateUser = ValidateUser(session);
        if (!validateUser) return (null, UnAuthorizeResponse.Forbidden);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        var validateSessionScope = ValidateSessionScope(session, userRoles);
        if (!validateSessionScope) return (null, UnAuthorizeResponse.Forbidden);
        var service = await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        if (service is null) return (null, UnAuthorizeResponse.Forbidden);
        if (InvalidRole()) return (null, UnAuthorizeResponse.Forbidden);

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

        return (new AuthorizeResponse(session.User.Id)
        {
            ScopeId = session.Scope.Id,
            ServiceId = service.Id
        }, null);
        bool InvalidRole() => userRoles.All(role => roles.All(r => role.Name != r.ToLower()));
    }

    private async Task<(Session?, UnAuthorizeResponse?)> ValidateSession(string token, CancellationToken cancellationToken)
    {
        var session = await _authorizeDataProvider.FindSessionAsync(token, cancellationToken);
        if (session is null)
        {
            return (null, UnAuthorizeResponse.UnAuthorized);
        }

        if (IsSessionExpired()) return (null, UnAuthorizeResponse.SessionExpired);

        return (session, null);

        bool IsSessionExpired() => DateTime.UtcNow >= session.ExpirationDateUtc;
    }

    private bool ValidateUser(Session session)
    {
        if (UserIsNotActive()) return false;
        return true;
        bool UserIsNotActive() => session.User.Status != UserStatus.Active;
    }

    private bool ValidateSessionScope(Session session, IEnumerable<Role> userRoles)
    {
        if (UserHasNotScopeRole()) return false;
        return true;
        bool UserHasNotScopeRole() => !userRoles.Any(role => session.Scope.Roles.Any(r => r.Id == role.Id));
    }

    private async Task<Service?> ValidateRequestedService(string serviceSecret, Scope sessionScope, CancellationToken cancellationToken)
    {
        var service = await _authorizeDataProvider.FindServiceAsync(serviceSecret, cancellationToken);
        if (service is not null && !InvalidSessionScopeService()) return service;
        if (service is null || service.Name != EntityBaseValues.KunderaServiceName)
        {
            return null;
        }

        return service;
        bool InvalidSessionScopeService() => sessionScope.Services.All(s => s.Id != service.Id);
    }
}