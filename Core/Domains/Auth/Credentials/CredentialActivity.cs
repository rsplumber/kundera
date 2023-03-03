using System.Net;

namespace Core.Domains.Auth.Credentials;

public class CredentialActivity : BaseEntity
{
    public CredentialActivity()
    {
    }

    internal CredentialActivity(Credential credential,IPAddress ipAddress,string agent)
    {
        Credential = credential;
        IpAddress = ipAddress.ToString();
        Agent = agent;
        CreatedDateUtc = DateTime.UtcNow;
    }


    public Guid Id { get;  init; } = Guid.NewGuid();

    public Credential Credential { get; init; }

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
    
    public DateTime CreatedDateUtc { get; init; }
    
}