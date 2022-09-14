using System.Net;
using Authorization.Domain.Types;
using Tes.Domain.Contracts;

namespace Authorization.Domain;

public class Session : AggregateRoot<Token>
{
    private readonly string _refreshToken;
    private readonly string _scope;
    private readonly Guid _userId;
    private readonly DateTime _expireDate;
    private DateTime _lastUsageDate;
    private string? _lastIpAddress;

    protected Session()
    {
    }

    private Session(Token token, Token refreshToken, string scope, Guid userId, DateTime expireDate, string? lastIpAddress = null) : base(token)
    {
        _refreshToken = refreshToken.Value;
        _scope = scope;
        _userId = userId;
        _expireDate = expireDate;
        _lastIpAddress = lastIpAddress;
        _lastUsageDate = DateTime.UtcNow;
    }

    public static Session Create(Token token, Token refreshToken, string scope, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        return new Session(token, refreshToken, scope, userId, expireDate, lastIpAddress?.ToString());
    }

    public string RefreshToken => _refreshToken;

    public string Scope => _scope;

    public Guid UserId => _userId;

    public DateTime LastUsageDate => _lastUsageDate;

    public DateTime ExpireDate => _expireDate;

    public string LastIpAddress => _lastIpAddress;

    public void UpdateUsage(DateTime lastUsageDate, IPAddress ipAddress)
    {
        _lastIpAddress = ipAddress.ToString();
        _lastUsageDate = lastUsageDate;
    }
}