using Core.Domains.Auth.Sessions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;

namespace Core.Domains.Auth.Authorizations;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly IAuthorizeDataProvider _authorizeDataProvider;

    public AuthorizeService(IAuthorizeDataProvider authorizeDataProvider)
    {
        _authorizeDataProvider = authorizeDataProvider;
    }

    public async Task<Guid> AuthorizePermissionAsync(string token, IEnumerable<string> actions, string serviceSecret, string userAgent, CancellationToken cancellationToken = default)
    {
        var session = await ValidateSession(token, userAgent, cancellationToken);
        ValidateUser(session);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        ValidateSessionScope(session, userRoles);
        var _ = await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        var permissions = await _authorizeDataProvider.RolePermissionsAsync(userRoles, cancellationToken);
        if (InvalidPermission())
        {
            throw new ForbiddenException();
        }

        return session.User.Id;

        bool InvalidPermission() => permissions.All(permission => actions.All(action => permission.Name != action.ToLower()));
    }

    public async Task<Guid> AuthorizeRoleAsync(string token, IEnumerable<string> roles, string serviceSecret, string userAgent,
        CancellationToken cancellationToken = default)
    {
        var session = await ValidateSession(token, userAgent, cancellationToken);
        ValidateUser(session);
        var userRoles = await _authorizeDataProvider.UserRolesAsync(session.User, cancellationToken);
        ValidateSessionScope(session, userRoles);
        var _ = await ValidateRequestedService(serviceSecret, session.Scope, cancellationToken);
        if (InvalidRole())
        {
            throw new ForbiddenException();
        }

        return session.User.Id;
        bool InvalidRole() => userRoles.All(role => roles.All(r => role.Name != r.ToLower()));
    }

    private async Task<Session> ValidateSession(string token, string agent, CancellationToken cancellationToken)
    {
        var session = await _authorizeDataProvider.CurrentSessionAsync(token, cancellationToken);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (SessionExpired())
        {
            throw new SessionExpiredException();
        }

        if (SessionInvalidAgent())
        {
            throw new UnAuthorizedException();
        }

        return session;

        bool SessionExpired() => DateTime.UtcNow >= session.ExpirationDateUtc;

        // bool SessionInvalidAgent() =>  session.Activity.Agent != agent;
        bool SessionInvalidAgent() => false;
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