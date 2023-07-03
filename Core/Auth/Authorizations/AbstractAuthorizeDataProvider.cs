using Core.Auth.Sessions;
using Core.Roles;
using Core.Services;
using Core.Users;

namespace Core.Auth.Authorizations;

public abstract class AbstractAuthorizeDataProvider : IAuthorizeDataProvider
{
    public abstract Task<Session?> FindSessionAsync(string sessionToken, CancellationToken cancellationToken = default);

    public abstract Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default);

    public abstract Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default);
}