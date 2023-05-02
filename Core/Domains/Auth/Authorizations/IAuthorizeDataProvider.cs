using Core.Domains.Auth.Sessions;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Services;
using Core.Domains.Users;

namespace Core.Domains.Auth.Authorizations;

public interface IAuthorizeDataProvider
{
    Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default);

    Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default);

    Task<Permission[]> RolePermissionsAsync(List<Role> roles, CancellationToken cancellationToken = default);

    Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default);
}