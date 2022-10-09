using Redis.OM.Modeling;

namespace Authentication.Data.Redis;

[Document(IndexName = "credentials", StorageType = StorageType.Json, Prefixes = new[] {"credential"})]
internal sealed class CredentialDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    public Guid UserId { get; set; }

    public PasswordType Password { get; set; }

    public string LastIpAddress { get; set; }

    public DateTime LastLoggedIn { get; set; }

    public DateTime? ExpiresAt { get; set; }

    [Indexed] public bool OneTime { get; set; }
}

internal sealed class PasswordType
{
    public string Value { get; set; }

    public string Salt { get; set; }
}