using System.Net;

namespace Core.Domains.Auth;

public class AuthActivity : BaseEntity
{
    public AuthActivity()
    {
    }

    internal AuthActivity(IPAddress ipAddress, string agent)
    {
        IpAddress = ipAddress.ToString();
        Agent = agent;
        CreatedDateUtc = DateTime.UtcNow;
    }


    public Guid Id { get; init; } = Guid.NewGuid();

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }

    public DateTime CreatedDateUtc { get; init; }
}