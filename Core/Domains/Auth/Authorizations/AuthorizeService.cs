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
        var session = await ValidateSession(token, cancellationToken);
        var user = await ValidateUser(session,cancellationToken);
        var userRoles = await GetUserRoles(user, cancellationToken);
        var sessionScope = await ValidateSessionScope(session, userRoles, cancellationToken);
        var _ = await ValidateRequestedService(serviceSecret, sessionScope, cancellationToken);
        var permissions = await _authorizeDataProvider.RolePermissionsAsync(userRoles,cancellationToken);
        if (InvalidPermission())
        {
            throw new ForbiddenException();
        }

        return user.Id;
        bool InvalidPermission() => permissions.All(permission => actions.All(action => permission.Name != action.ToLower()));
    }

    public async Task<Guid> AuthorizeRoleAsync(string token, IEnumerable<string> roles, string serviceSecret, string userAgent,
        CancellationToken cancellationToken = default)
    {
        var session = await ValidateSession(token, cancellationToken);
        var user = await ValidateUser(session,cancellationToken);
        var userRoles = await GetUserRoles(user, cancellationToken);
        var sessionScope = await ValidateSessionScope(session, userRoles, cancellationToken);
        var _ = await ValidateRequestedService(serviceSecret, sessionScope, cancellationToken);
        if (InvalidRole())
        {
            throw new ForbiddenException();
        }

        return user.Id;
        bool InvalidRole() => userRoles.All(role => roles.All(r => role.Name != r.ToLower()));
    }
    
    private async Task<Session> ValidateSession(string token, CancellationToken cancellationToken)
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

        return session;
        
        bool SessionExpired() => DateTime.UtcNow >= session.ExpirationDateUtc;
    }
    
    private async Task<User> ValidateUser(Session session,CancellationToken cancellationToken)
    {
        var user = await _authorizeDataProvider.SessionUserAsync(session.UserId, cancellationToken);
        if (user is null || UserIsNotActive())
        {
            throw new ForbiddenException();
        }

        return user;

        bool UserIsNotActive() => user.Status.Name != UserStatus.Active.Name;
    }
    
    private async Task<IReadOnlySet<Role>> GetUserRoles(User user, CancellationToken cancellationToken)
    {
        return  await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
    }
    
    private async Task<Scope> ValidateSessionScope(Session session, IReadOnlySet<Role> userRoles, CancellationToken cancellationToken)
    {
        var sessionScope = await _authorizeDataProvider.SessionScopeAsync(session.ScopeId, cancellationToken);
        if (sessionScope is null || UserHasNotScopeRole())
        {
            throw new ForbiddenException();
        }

        return sessionScope;
        bool UserHasNotScopeRole() => !userRoles.Any(role => sessionScope.Roles.Any(id => id == role.Id));
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
        bool InvalidSessionScopeService() => sessionScope.Services.All(id => id != service.Id);
    }
}