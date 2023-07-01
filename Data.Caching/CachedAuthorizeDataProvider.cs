using System.Text.Json;
using Core.Auth.Authorizations;
using Core.Auth.Sessions;
using Core.Roles;
using Core.Services;
using Core.Users;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Caching;

internal sealed class CachedAuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly AbstractAuthorizeDataProvider _authorizeDataProvider;
    private readonly IDistributedCache _cacheService;
    private const string SessionsTypeKey = "sessions";
    private const string RolesTypeKey = "user_roles";
    private const string ServicesTypeKey = "services";

    private const string CacheKeyPrefix = "kundera";

    private static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromDays(15))
        .SetSlidingExpiration(TimeSpan.FromDays(5));

    public CachedAuthorizeDataProvider(AbstractAuthorizeDataProvider authorizeDataProvider, IDistributedCache cacheService)
    {
        _authorizeDataProvider = authorizeDataProvider;
        _cacheService = cacheService;
    }


    public async Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyFrom(SessionsTypeKey, sessionToken);
        var cachedSession = await _cacheService.GetAsync(cacheKey, cancellationToken);
        if (cachedSession is not null)
        {
            return JsonSerializer.Deserialize<Session>(cachedSession)!;
        }

        var session = await _authorizeDataProvider.CurrentSessionAsync(sessionToken, cancellationToken);
        if (session is null) return null;
        var sessionBytes = JsonSerializer.SerializeToUtf8Bytes(session);
        await _cacheService.SetAsync(cacheKey, sessionBytes, new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.UtcNow - session.ExpirationDateUtc), cancellationToken);
        return session;
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyFrom(RolesTypeKey, user.Id.ToString());
        var cachedRoles = await _cacheService.GetAsync(cacheKey, cancellationToken);
        if (cachedRoles is not null)
        {
            return JsonSerializer.Deserialize<List<Role>>(cachedRoles)!;
        }

        var roles = await _authorizeDataProvider.UserRolesAsync(user, cancellationToken);
        var rolesBytes = JsonSerializer.SerializeToUtf8Bytes(roles);
        await _cacheService.SetAsync(cacheKey, rolesBytes, DistributedCacheEntryOptions, cancellationToken);
        return roles;
    }

    public async Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyFrom(SessionsTypeKey, serviceSecret);
        var cachedService = await _cacheService.GetAsync(cacheKey, cancellationToken);
        if (cachedService is not null)
        {
            return JsonSerializer.Deserialize<Service>(cachedService)!;
        }

        var service = await _authorizeDataProvider.RequestedServiceAsync(serviceSecret, cancellationToken);
        var serviceBytes = JsonSerializer.SerializeToUtf8Bytes(service);
        await _cacheService.SetAsync(cacheKey, serviceBytes, DistributedCacheEntryOptions, cancellationToken);
        return service;
    }

    private static string CacheKeyFrom(string type, string key) => $"{CacheKeyPrefix}_{type}:{key}";
}