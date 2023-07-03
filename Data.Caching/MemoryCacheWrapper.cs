using Microsoft.Extensions.Caching.Distributed;

namespace Data.Caching;

public abstract class MemoryCacheWrapper : IDistributedCache
{
    internal readonly List<string> CacheKeys = new();

    public abstract byte[]? Get(string key);

    public abstract Task<byte[]?> GetAsync(string key, CancellationToken token = new CancellationToken());

    public abstract void Set(string key, byte[] value, DistributedCacheEntryOptions options);

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = new CancellationToken())
    {
        CacheKeys.Remove(key);
        CacheKeys.Add(key);
        Set(key, value, options);
        return Task.CompletedTask;
    }

    public abstract void Refresh(string key);

    public abstract Task RefreshAsync(string key, CancellationToken token = new CancellationToken());

    public abstract void Remove(string key);

    public Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
    {
        CacheKeys.Remove(key);
        Remove(key);
        return Task.CompletedTask;
    }
}