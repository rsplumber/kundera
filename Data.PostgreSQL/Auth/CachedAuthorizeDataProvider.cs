using Core.Auth.Authorizations;
using Core.Auth.Sessions;
using Core.Permissions;
using Core.Roles;
using Core.Services;
using Core.Users;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Auth;

internal sealed class CachedAuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly AuthorizeDataProvider _authorizeDataProvider;
    private readonly IDistributedCache _cache;

    public CachedAuthorizeDataProvider(AuthorizeDataProvider authorizeDataProvider, IDistributedCache cache)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _cache = cache;
    }

    public async Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        // var cacheData = await _cache.GetAsync(sessionToken, cancellationToken);
        // if (cacheData is not null)
        // {
        //     return JsonSerializer.Deserialize<Session>(cacheData);
        // }
        //
        // var currentSession = await _authorizeDataProvider.CurrentSessionAsync(sessionToken, cancellationToken);
        // await _cache.SetStringAsync(sessionToken, JsonSerializer.Serialize(currentSession), cancellationToken);
        return await _authorizeDataProvider.CurrentSessionAsync(sessionToken, cancellationToken);
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
    }

    public async Task<Permission[]> RolePermissionsAsync(List<Role> roles, CancellationToken cancellationToken = default)
    {
        return await _authorizeDataProvider.RolePermissionsAsync(roles, cancellationToken);
    }

    public async Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        return await _authorizeDataProvider.RequestedServiceAsync(serviceSecret, cancellationToken);
    }
}