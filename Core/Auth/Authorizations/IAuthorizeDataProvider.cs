using Core.Auth.Sessions;
using Core.Roles;
using Core.Services;
using Core.Users;

namespace Core.Auth.Authorizations;

public interface IAuthorizeDataProvider
{
    Task<Session?> FindSessionAsync(string sessionToken, CancellationToken cancellationToken = default);

    Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default);

    Task<Service?> FindServiceAsync(string serviceSecret, CancellationToken cancellationToken = default);
}