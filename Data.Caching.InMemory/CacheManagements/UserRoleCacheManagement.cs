using System.Text.Json;
using Core.Roles;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Caching.InMemory.CacheManagements;

internal sealed class UserRoleCacheManagement
{
    private const string RolesTypeKey = "user_roles";
    private readonly IDistributedCache _cacheService;

    private static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromDays(30))
        .SetSlidingExpiration(TimeSpan.FromDays(10));

    public UserRoleCacheManagement(IDistributedCache cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask<List<Role>?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(RolesTypeKey, userId.ToString());
        var cachedData = await _cacheService.GetAsync(key, cancellationToken);
        return cachedData is not null ? JsonSerializer.Deserialize<List<Role>>(cachedData) : null;
    }

    public Task SetAsync(Guid userId, List<Role> roles, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(RolesTypeKey, userId.ToString());
        var bytes = JsonSerializer.SerializeToUtf8Bytes(roles);
        return _cacheService.SetAsync(key, bytes, DistributedCacheEntryOptions, cancellationToken);
    }

    public Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(RolesTypeKey, userId.ToString());
        return _cacheService.RemoveAsync(key, cancellationToken);
    }
}