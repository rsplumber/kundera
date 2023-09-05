using System.Text.Json;
using Core.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Caching.InMemory.CacheManagements;

internal sealed class ServiceCacheManagement
{
    private const string ServicesTypeKey = "services";
    private readonly IDistributedCache _cacheService;

    private static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromDays(30))
        .SetSlidingExpiration(TimeSpan.FromDays(10));

    public ServiceCacheManagement(IDistributedCache cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask<Service?> GetAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(ServicesTypeKey, serviceSecret);
        var cachedData = await _cacheService.GetAsync(key, cancellationToken);
        return cachedData is not null ? JsonSerializer.Deserialize<Service>(cachedData) : null;
    }

    public Task SetAsync(Service service, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(ServicesTypeKey, service.Secret);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(service);
        return _cacheService.SetAsync(key, bytes, DistributedCacheEntryOptions, cancellationToken);
    }

    public Task DeleteAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(ServicesTypeKey, serviceSecret);
        return _cacheService.RemoveAsync(key, cancellationToken);
    }
}