using System.Net;
using Core.Domains.Auth.Credentials;

namespace Core.Domains.Auth.Sessions;

public class SessionActivity : BaseEntity
{
    public SessionActivity()
    {
    }

    internal SessionActivity(Session session,IPAddress ipAddress,string agent)
    {
        Credential = session;
        IpAddress = ipAddress.ToString();
        Agent = agent;
        CreatedDateUtc = DateTime.UtcNow;
    }


    public Guid Id { get; init; } = Guid.NewGuid();

    public Session Credential { get; init; } = default!;

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
    
    public DateTime CreatedDateUtc { get; init; }
    
}