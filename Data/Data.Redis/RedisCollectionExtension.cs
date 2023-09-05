using Data.Abstractions;
using Redis.OM;
using Redis.OM.Searching;

namespace Data;

internal static class RedisCollectionExtension
{
    public static IRedisCollection<TEntity> Page<TEntity>(this IRedisCollection<TEntity> redisCollection, PageableQuery query) where TEntity : class
    {
        return redisCollection.Skip((query.Page - 1) * query.Size)
            .Take(query.Size);
    }
}