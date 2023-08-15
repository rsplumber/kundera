using Data.Abstractions;

namespace Data;

internal static class QueryableExtension
{
    public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity?> dbSet, PageableQuery query) where TEntity : class
    {
        return dbSet.Skip((query.Page - 1) * query.Size)
            .Take(query.Size)!;
    }
}