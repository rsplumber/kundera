using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Data.Caching;

internal static class DistributedCacheExtensions
{
    public static Task ClearDb(this IDistributedCache distributedCache, string dbType, IServiceProvider serviceProvider)
    {
        switch (dbType)
        {
            case "InMemory":
                var memoryCache = serviceProvider.GetRequiredService<IDistributedCache>();
                if (memoryCache is not MemoryCacheWrapper concreteMemoryCache) return Task.CompletedTask;
                foreach (var ktr in concreteMemoryCache.CacheKeys)
                {
                    memoryCache.Remove(ktr);
                }

                return Task.CompletedTask;
            case "Redis":
                var redisServer = serviceProvider.GetRequiredService<IServer>();
                return redisServer.FlushDatabaseAsync();
            default:
                return Task.CompletedTask;
        }
    }

    public static Task ClearDb(this IDistributedCache distributedCache, string dbType, string patternPrefix, IServiceProvider serviceProvider)
    {
        switch (dbType)
        {
            case "InMemory":
                var memoryCache = serviceProvider.GetRequiredService<IDistributedCache>();
                if (memoryCache is not MemoryCacheWrapper concreteMemoryCache) return Task.CompletedTask;
                foreach (var ktr in concreteMemoryCache.CacheKeys
                             .Where(k => Regex.IsMatch(k, patternPrefix + "*", RegexOptions.IgnoreCase)))
                {
                    memoryCache.Remove(ktr);
                }

                return Task.CompletedTask;
            case "Redis":
                var redisServer = serviceProvider.GetRequiredService<IServer>();
                foreach (var key in redisServer.Keys(pattern: $"{patternPrefix}*"))
                {
                    redisServer.Multiplexer.GetDatabase().KeyDelete(key);
                }

                return Task.CompletedTask;
            default:
                return Task.CompletedTask;
        }
    }
}