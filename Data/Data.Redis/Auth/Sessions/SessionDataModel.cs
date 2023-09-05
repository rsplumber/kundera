using Redis.OM.Modeling;

namespace Data.Auth.Sessions;

[Document(IndexName = "sessions", StorageType = StorageType.Json, Prefixes = new[] { "sessions" })]
internal sealed class SessionDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; } = default!;

    [Indexed] public string RefreshToken { get; set; } = default!;

    [Indexed] public Guid ScopeId { get; set; } = default!;

    [Indexed] public Guid UserId { get; set; } = default!;

    [Indexed] public Guid CredentialId { get; set; } = default!;

    public AuthorizationActivityDataModel Activity { get; internal set; } = default!;
}

internal sealed class AuthorizationActivityDataModel
{
    public Guid Id { get; set; }

    public string Session { get; set; } = default!;

    public string? IpAddress { get; internal set; }

    public string? Agent { get; internal set; }

    public DateTime CreatedDateUtc { get; internal set; }
}