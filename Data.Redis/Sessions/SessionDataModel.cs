using Redis.OM.Modeling;

namespace Managements.Data.Sessions;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] {"sessions"})]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    public string RefreshToken { get; set; }

    [Indexed] public Guid ScopeId { get; set; }

    [Indexed] public Guid UserId { get; set; }

    [Indexed] public DateTime ExpiresAt { get; set; }

    [Indexed] public DateTime CreatedDate { get; set; }

    [Indexed] public DateTime LastUsageDate { get; set; }

    public string? LastIpAddress { get; set; }
}