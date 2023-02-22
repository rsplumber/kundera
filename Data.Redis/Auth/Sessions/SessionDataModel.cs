using Redis.OM.Modeling;

namespace Managements.Data.Auth.Sessions;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] { "sessions" })]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; } = default!;

    [Indexed] public string RefreshToken { get; set; } = default!;

    [Indexed] public Guid ScopeId { get; set; } = default!;

    [Indexed] public Guid UserId { get; set; } = default!;

    [Indexed] public Guid CredentialId { get; set; } = default!;

    [Indexed(Sortable = true)] public DateTime ExpirationDateUtc { get; set; } = default!;

    [Indexed(Sortable = true)] public DateTime CreatedDateUtc { get; set; } = default!;

    [Indexed(Sortable = true)] public DateTime LastUsageDateUtc { get; set; } = default!;

    [Indexed] public string? LastIpAddress { get; set; }
}