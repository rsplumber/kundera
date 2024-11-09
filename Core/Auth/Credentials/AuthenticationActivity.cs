namespace Core.Auth.Credentials;

public class AuthenticationActivity : BaseEntity
{
    public AuthenticationActivity()
    {
    }

    public AuthenticationActivity(Guid credentialId,
        Guid userId,
        Guid scopeId,
        string username,
        string? ipAddress,
        string? agent,
        string? platform)
    {
        Credential = credentialId;
        UserId = userId;
        IpAddress = ipAddress;
        Agent = agent;
        Username = username;
        ScopeId = scopeId;
        Platform = platform;
    }


    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid Credential { get; init; }

    public Guid UserId { get; init; }

    public Guid ScopeId { get; init; }

    public string Username { get; init; } = default!;

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }

    public string? Platform { get; init; }

    public DateTime CreatedDateUtc { get; init; } = DateTime.UtcNow;
}