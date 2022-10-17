using Redis.OM.Modeling;

namespace Auth.Data.Redis;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] {"sessions"})]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    public string RefreshToken { get; set; }

    [Searchable] public string Scope { get; set; }

    [Indexed] public Guid UserId { get; set; }

    [Indexed] public DateTime ExpiresAt { get; set; }

    [Indexed] public DateTime CreatedDate { get; set; }

    [Indexed] public DateTime LastUsageDate { get; set; }

    public string? LastIpAddress { get; set; }
}