using Redis.OM.Modeling;

namespace Authorization.Data.Redis;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] {"session"})]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    public string RefreshToken { get; set; }

    [Indexed] [Searchable] public string Scope { get; set; }

    [Indexed] public string User { get; set; }

    public string? StatusChangedReason { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime LastUsageDate { get; set; }

    public string? LastIpAddress { get; set; }
}