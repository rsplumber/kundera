using Redis.OM.Modeling;

namespace Data.Auth.Credentials;

[Document(IndexName = "credentials", StorageType = StorageType.Json, Prefixes = new[] { "credentials" })]
internal sealed class CredentialDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed]
    [Searchable(PropertyName = "username_searchable")]
    public string Username { get; set; } = default!;

    [Indexed] public Guid UserId { get; set; }

    public PasswordType Password { get; set; } = default!;
    
    public AuthenticationActivityDataModel? FirstActivity { get; internal set; }
    
    public AuthenticationActivityDataModel? LastActivity { get; internal set; }
    
    [Indexed(Sortable = true)] public DateTime? ExpiresAtUtc { get; set; }

    [Indexed(Sortable = true)] public DateTime CreatedDateUtc { get; set; }

    [Indexed] public bool OneTime { get; set; }

    [Indexed] public int? SessionExpireTimeInMinutes { get; set; }

    [Indexed] public bool SingleSession { get; set; }
}

internal sealed class PasswordType
{
    public string Value { get; set; } = default!;

    public string Salt { get; set; } = default!;
}

internal sealed class AuthenticationActivityDataModel
{
    public Guid Id { get;  set; }

    public Guid CredentialId { get;  set; }

    public string? IpAddress { get; internal set; }

    public string? Agent { get; internal set; }
    
    public DateTime CreatedDateUtc { get; internal set; }
}