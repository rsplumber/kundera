using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions.Events;
using Core.Domains.Scopes;
using Core.Domains.Users;

namespace Core.Domains.Auth.Sessions;

public class Session : BaseEntity
{
    public Session()
    {
    }

    internal Session(string token,
        string refreshToken,
        Credential credential,
        Scope scope,
        User user,
        DateTime expireDate,
        IPAddress ipAddress,
        string agent)
    {
        Id = token;
        RefreshToken = refreshToken;
        Credential = credential;
        Scope = scope;
        User = user;
        ExpirationDateUtc = expireDate;
        UpdateActivity(ipAddress,agent);
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public string Id { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public Credential Credential { get; set; }

    public Scope Scope { get; set; }

    public User User { get; set; }

    public DateTime ExpirationDateUtc { get; set; }
    
    public AuthActivity Activity { get; set; }

    public void UpdateActivity(IPAddress ipAddress,string agent)
    {
        Activity = new AuthActivity(ipAddress, agent);
    }

}