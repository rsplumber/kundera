namespace Core.Domains.Auth.Sessions;

public interface ISessionFactory
{
    public Task<Session> CreateAsync(string token,
        string refreshToken,
        Guid credentialId,
        Guid scopeId,
        Guid userId,
        DateTime expireDate,
        string agent);
}

internal sealed class SessionFactory : ISessionFactory
{
    private readonly ISessionRepository _sessionRepository;

    public SessionFactory(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> CreateAsync(string token,
        string refreshToken,
        Guid credentialId,
        Guid scopeId,
        Guid userId,
        DateTime expireDate,
        string agent)
    {
        var session = new Session(token, refreshToken, credentialId, scopeId, userId, expireDate)
        {
            UserAgent = agent
        };
        await _sessionRepository.AddAsync(session);
        return session;
    }
}