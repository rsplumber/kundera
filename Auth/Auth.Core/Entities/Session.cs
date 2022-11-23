using System.Net;
using Auth.Core.Events;
using Managements.Domain.Contracts;

namespace Auth.Core.Entities;

public class Session : Entity
{
    protected Session()
    {
    }

    internal Session(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        Id = token;
        RefreshToken = refreshToken;
        ScopeId = scopeId;
        UserId = userId;
        ExpiresAt = expireDate;
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastUsageDate = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public Token Id { get; internal set; } = default!;

    public Token RefreshToken { get; internal set; } = default!;
    public Guid ScopeId { get; internal set; }

    public Guid UserId { get; internal set; }

    public DateTime ExpiresAt { get; internal set; }

    public DateTime LastUsageDate { get; internal set; }

    public IPAddress LastIpAddress { get; internal set; } = default!;

    public DateTime CreatedDate { get; internal set; }


    public void UpdateUsage(DateTime lastUsageDate, IPAddress? ipAddress)
    {
        LastIpAddress = ipAddress ?? IPAddress.None;
        LastUsageDate = lastUsageDate;
    }
}