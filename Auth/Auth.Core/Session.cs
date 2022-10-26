using System.Net;
using Auth.Core.Events;
using Kite.Domain.Contracts;

namespace Auth.Core;

public class Session : AggregateRoot<Token>
{
    protected Session()
    {
    }

    internal Session(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null) : base(token)
    {
        RefreshToken = refreshToken;
        ScopeId = scopeId;
        UserId = userId;
        ExpiresAt = expireDate;
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastUsageDate = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }


    public Token RefreshToken { get; internal set; }
    public Guid ScopeId { get; internal set; }

    public Guid UserId { get; internal set; }

    public DateTime ExpiresAt { get; internal set; }

    public DateTime LastUsageDate { get; internal set; }

    public IPAddress LastIpAddress { get; internal set; }

    public DateTime CreatedDate { get; internal set; }


    public void UpdateUsage(DateTime lastUsageDate, IPAddress? ipAddress)
    {
        LastIpAddress = ipAddress ?? IPAddress.None;
        LastUsageDate = lastUsageDate;
    }
}