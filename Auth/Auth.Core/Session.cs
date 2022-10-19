using System.Net;
using Auth.Core.Events;
using Kite.Domain.Contracts;

namespace Auth.Core;

public class Session : AggregateRoot<Token>
{
    private Token _refreshToken;
    private Guid _scopeId;
    private Guid _userId;
    private DateTime _expiresAt;
    private DateTime _lastUsageDate;
    private IPAddress? _lastIpAddress;
    private DateTime _createdDate;

    protected Session()
    {
    }

    private Session(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null) : base(token)
    {
        _refreshToken = refreshToken;
        _scopeId = scopeId;
        _userId = userId;
        _expiresAt = expireDate;
        _lastIpAddress = lastIpAddress;
        _lastUsageDate = DateTime.UtcNow;
        _createdDate = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public static Session Create(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        return new Session(token, refreshToken, scopeId, userId, expireDate, lastIpAddress);
    }

    public Token RefreshToken => _refreshToken;

    public Guid ScopeId => _scopeId;

    public Guid UserId => _userId;

    public DateTime LastUsageDate => _lastUsageDate;

    public DateTime ExpiresAt => _expiresAt;

    public IPAddress? LastIpAddress => _lastIpAddress;

    public DateTime CreatedDate => _createdDate;


    public void UpdateUsage(DateTime lastUsageDate, IPAddress ipAddress)
    {
        _lastIpAddress = ipAddress;
        _lastUsageDate = lastUsageDate;
    }
}