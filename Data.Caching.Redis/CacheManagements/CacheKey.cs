namespace Data.Caching.Redis.CacheManagements;

internal static class CacheKey
{
    private const string CacheKeyPrefix = "kundera";
    internal static string From(string type, string key) => $"{CacheKeyPrefix}_{type}:{key}";
}