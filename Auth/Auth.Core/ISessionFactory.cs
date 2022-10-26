using System.Net;

namespace Auth.Core;

public interface ISessionFactory
{
    public Task<Session> CreateAsync(Token token,
        Token refreshToken,
        Guid scopeId,
        Guid userId,
        DateTime expireDate,
        IPAddress? lastIpAddress = null);
}

internal sealed class SessionFactory : ISessionFactory
{
    public Task<Session> CreateAsync(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        return Task.FromResult(new Session(token, refreshToken, scopeId, userId, expireDate, lastIpAddress));
    }
}