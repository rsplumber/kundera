using System.Text.Json;
using Core.Auth.Sessions;
using Microsoft.Extensions.Caching.Distributed;

namespace Data.Caching.InMemory.CacheManagements;

internal sealed class SessionCacheManagement
{
    private const string SessionsTypeKey = "sessions";
    private readonly IDistributedCache _cacheService;

    private static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions = new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromDays(6))
        .SetSlidingExpiration(TimeSpan.FromDays(2));

    public SessionCacheManagement(IDistributedCache cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask<Session?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(SessionsTypeKey, token);
        var cachedData = await _cacheService.GetAsync(key, cancellationToken);
        return cachedData is not null ? JsonSerializer.Deserialize<Session>(cachedData) : null;
    }

    public Task SetAsync(Session session, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(SessionsTypeKey, session.Id);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(session);
        return _cacheService.SetAsync(key, bytes, DistributedCacheEntryOptions, cancellationToken);
    }

    public Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.From(SessionsTypeKey, token);
        return _cacheService.RemoveAsync(key, cancellationToken);
    }
}