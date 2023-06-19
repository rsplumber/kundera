using Core.Auth.Sessions;
using Core.Permissions;
using Core.Roles;
using Core.Services;
using Core.Users;

namespace Core.Auth.Authorizations;

public interface IAuthorizeDataProvider
{
    Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default);

    Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default);

    Task<Permission[]> RolePermissionsAsync(List<Role> roles, CancellationToken cancellationToken = default);

    Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default);
}