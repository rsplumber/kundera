using Redis.OM.Modeling;

namespace Managements.Data.Auth.Sessions;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] {"sessions"})]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    [Indexed] public Guid ScopeId { get; set; } = default!;

    [Indexed] public Guid UserId { get; set; } = default!;

    [Indexed] public DateTime ExpiresAt { get; set; } = default!;

    [Indexed] public DateTime CreatedDate { get; set; } = default!;

    [Indexed] public DateTime LastUsageDate { get; set; } = default!;

    public string? LastIpAddress { get; set; }
}