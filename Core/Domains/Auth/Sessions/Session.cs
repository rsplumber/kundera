using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions.Events;
using Core.Domains.Scopes.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Sessions;

public class Session : Entity
{
    protected Session()
    {
    }

    internal Session(Token token,
        Token refreshToken,
        CredentialId credentialId,
        ScopeId scope,
        UserId user,
        DateTime expireDate,
        IPAddress? lastIpAddress = null)
    {
        Id = token;
        RefreshToken = refreshToken;
        Credential = credentialId;
        Scope = scope;
        User = user;
        ExpirationDateUtc = expireDate;
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastUsageDateUtc = DateTime.UtcNow;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public Token Id { get; internal set; } = default!;

    public Token RefreshToken { get; internal set; } = default!;

    public CredentialId Credential { get; internal set; } = default!;

    public ScopeId Scope { get; internal set; } = default!;

    public UserId User { get; internal set; } = default!;

    public DateTime ExpirationDateUtc { get; internal set; }

    public DateTime LastUsageDateUtc { get; internal set; }

    public IPAddress LastIpAddress { get; internal set; } = default!;

    public DateTime CreatedDateUtc { get; internal set; }


    public void UpdateUsage(DateTime lastUsageDate, IPAddress? ipAddress)
    {
        LastIpAddress = ipAddress ?? IPAddress.None;
        LastUsageDateUtc = lastUsageDate;
    }
}