using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions.Events;
using Core.Domains.Scopes;
using Core.Domains.Users;

namespace Core.Domains.Auth.Sessions;

public class Session : BaseEntity
{
    protected Session()
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

    public string Id { get; internal set; } = default!;

    public string RefreshToken { get; internal set; } = default!;

    public Credential Credential { get; internal set; }

    public Scope Scope { get; internal set; }

    public User User { get; internal set; }

    public DateTime ExpirationDateUtc { get; internal set; }
    
    public AuthActivity Activity { get; internal set; }

    public void UpdateActivity(IPAddress ipAddress,string agent)
    {
        Activity = new AuthActivity(ipAddress, agent);
    }

}