using Core.Domains.Auth.Sessions;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;

namespace Core.Domains.Auth.Authorizations;

public interface IAuthorizeDataProvider
{
    Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default);
    
    Task<User?> SessionUserAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlySet<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default);
    
    Task<Scope?> SessionScopeAsync(Guid scopeId, CancellationToken cancellationToken = default);
    
    Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default);
    
    Task<IReadOnlySet<Permission>> RolePermissionsAsync(IReadOnlySet<Role> roles, CancellationToken cancellationToken = default);
}