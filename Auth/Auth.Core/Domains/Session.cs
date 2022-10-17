using System.Net;
using Auth.Core.Events;
using Kite.Domain.Contracts;

namespace Auth.Core.Domains;

public class Session : AggregateRoot<Token>
{
    private Token _refreshToken;
    private string _scope;
    private Guid _userId;
    private DateTime _expiresAt;
    private DateTime _lastUsageDate;
    private string? _lastIpAddress;
    private DateTime _createdDate;

    protected Session()
    {
    }

    private Session(Token token, Token refreshToken, string scope, Guid userId, DateTime expireDate, string? lastIpAddress = null) : base(token)
    {
        _refreshToken = refreshToken;
        _scope = scope;
        _userId = userId;
        _expiresAt = expireDate;
        _lastIpAddress = lastIpAddress;
        _lastUsageDate = DateTime.UtcNow;
        _createdDate = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public static Session Create(Token token, Token refreshToken, string scope, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        return new Session(token, refreshToken, scope, userId, expireDate, lastIpAddress?.ToString());
    }

    public Token RefreshToken => _refreshToken;

    public string Scope => _scope;

    public Guid UserId => _userId;

    public DateTime LastUsageDate => _lastUsageDate;

    public DateTime ExpiresAt => _expiresAt;

    public string LastIpAddress => _lastIpAddress;

    public DateTime CreatedDate => _createdDate;


    public void UpdateUsage(DateTime lastUsageDate, IPAddress ipAddress)
    {
        _lastIpAddress = ipAddress.ToString();
        _lastUsageDate = lastUsageDate;
    }
}