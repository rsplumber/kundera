using Redis.OM.Modeling;

namespace Managements.Data.Auth.Credentials;

[Document(IndexName = "credentials", StorageType = StorageType.Json, Prefixes = new[] {"credentials"})]
internal sealed class CredentialDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; } = default!;

    [Indexed] public Guid UserId { get; set; }

    public PasswordType Password { get; set; } = default!;

    public string? LastIpAddress { get; set; }

    [Indexed] public DateTime LastLoggedIn { get; set; }

    [Indexed] public DateTime? ExpiresAt { get; set; }

    [Indexed] public DateTime CreatedDate { get; set; }

    [Indexed] public bool OneTime { get; set; }
}

internal sealed class PasswordType
{
    public string Value { get; set; } = default!;

    public string Salt { get; set; } = default!;
}