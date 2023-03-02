using System.Net;
using Core.Domains.Auth.Credentials.Events;

namespace Core.Domains.Auth.Credentials;

public class CredentialActivity : BaseEntity
{
    public CredentialActivity()
    {
    }

    internal CredentialActivity(Guid credentialId,IPAddress ipAddress,string agent)
    {
        CredentialId = credentialId;
        IpAddress = ipAddress.ToString();
        Agent = agent;
        CreatedDateUtc = DateTime.UtcNow;
    }


    public Guid Id { get;  init; } = Guid.NewGuid();

    public Guid CredentialId { get; init; }

    public string? IpAddress { get; init; }

    public string? Agent { get; init; }
    
    public DateTime CreatedDateUtc { get; init; }
    
}