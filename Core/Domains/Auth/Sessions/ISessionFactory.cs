using Core.Domains.Auth.Credentials;
using Core.Domains.Scopes.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Sessions;

public interface ISessionFactory
{
    public Task<Session> CreateAsync(Token token,
        Token refreshToken,
        CredentialId credentialId,
        ScopeId scope,
        UserId user,
        DateTime expireDate);
}

internal sealed class SessionFactory : ISessionFactory
{
    private readonly ISessionRepository _sessionRepository;

    public SessionFactory(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> CreateAsync(Token token,
        Token refreshToken,
        CredentialId credentialId,
        ScopeId scopeId,
        UserId userId,
        DateTime expireDate)
    {
        var session = new Session(token, refreshToken, credentialId, scopeId, userId, expireDate);
        await _sessionRepository.AddAsync(session);
        return session;
    }
}