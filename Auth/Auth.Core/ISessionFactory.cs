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
    private readonly ISessionRepository _sessionRepository;

    public SessionFactory(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> CreateAsync(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate, IPAddress? lastIpAddress = null)
    {
        var session = new Session(token, refreshToken, scopeId, userId, expireDate, lastIpAddress);
        await _sessionRepository.AddAsync(session);
        return session;
    }
}