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

    [Indexed] public string LastIpAddress { get; set; } = default!;

    [Indexed(Sortable = true)] public DateTime? LastLoggedInUtc { get; set; }

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