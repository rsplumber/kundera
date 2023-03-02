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
    
    [Indexed(Sortable = true)] public DateTime ExpirationDateUtc { get; set; } = default!;

    public SessionActivityDataModel Activity { get; internal set; } = default!;
}


internal sealed class SessionActivityDataModel
{
    public Guid Id { get;  set; }

    public Guid CredentialId { get;  set; } = default!;

    public string? IpAddress { get; internal set; }

    public string? Agent { get; internal set; }
    
    public DateTime CreatedDateUtc { get; internal set; }
}