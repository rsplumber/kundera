namespace Core.Domains.Auth.Sessions;

public class AuthorizationActivity : BaseEntity
{
    public AuthorizationActivity()
    {
    }

    public AuthorizationActivity(string session, Guid userId, string? ipAddress, string? agent)
    {
        Session = session;
        IpAddress = ipAddress;
        Agent = agent;
        UserId = userId;
    }


    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }

    public string Session { get; init; } = default!;

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }

    public DateTime CreatedDateUtc { get; init; } = DateTime.UtcNow;
}