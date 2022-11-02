namespace Auth.Core;

public interface ISessionFactory
{
    public Task<Session> CreateAsync(Token token,
        Token refreshToken,
        Guid scopeId,
        Guid userId,
        DateTime expireDate);
}

internal sealed class SessionFactory : ISessionFactory
{
    private readonly ISessionRepository _sessionRepository;

    public SessionFactory(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> CreateAsync(Token token, Token refreshToken, Guid scopeId, Guid userId, DateTime expireDate)
    {
        var session = new Session(token, refreshToken, scopeId, userId, expireDate);
        await _sessionRepository.AddAsync(session);
        return session;
    }
}