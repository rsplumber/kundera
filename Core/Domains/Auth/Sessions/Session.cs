using System.Net;
using Core.Domains.Auth.Sessions.Events;

namespace Core.Domains.Auth.Sessions;

public class Session : BaseEntity
{
    protected Session()
    {
    }

    internal Session(string token,
        string refreshToken,
        Guid credentialId,
        Guid scopeId,
        Guid userId,
        DateTime expireDate)
    {
        Id = token;
        RefreshToken = refreshToken;
        CredentialId = credentialId;
        ScopeId = scopeId;
        UserId = userId;
        ExpirationDateUtc = expireDate;
        LastUsageDateUtc = DateTime.UtcNow;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public string Id { get; internal set; } = default!;

    public string RefreshToken { get; internal set; } = default!;

    public Guid CredentialId { get; internal set; }

    public Guid ScopeId { get; internal set; }

    public Guid UserId { get; internal set; }

    public DateTime ExpirationDateUtc { get; internal set; }

    public DateTime LastUsageDateUtc { get; internal set; }

    public string? LastIpAddress { get; internal set; }

    public DateTime CreatedDateUtc { get; internal set; }


    public void UpdateUsage(DateTime lastUsageDate, IPAddress? ipAddress)
    {
        LastIpAddress = ipAddress?.ToString();
        LastUsageDateUtc = lastUsageDate;
    }
}