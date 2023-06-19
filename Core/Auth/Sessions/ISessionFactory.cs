using Core.Auth.Credentials;
using Core.Auth.Credentials.Exceptions;
using Core.Scopes;
using Core.Scopes.Exceptions;

namespace Core.Auth.Sessions;

public interface ISessionFactory
{
    public Task<Session> CreateAsync(string token,
        string refreshToken,
        Guid credentialId,
        Guid scopeId,
        DateTime expireDate);
}

internal sealed class SessionFactory : ISessionFactory
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly IScopeRepository _scopeRepository;

    public SessionFactory(ISessionRepository sessionRepository,
        ICredentialRepository credentialRepository,
        IScopeRepository scopeRepository)
    {
        _sessionRepository = sessionRepository;
        _credentialRepository = credentialRepository;
        _scopeRepository = scopeRepository;
    }

    public async Task<Session> CreateAsync(string token,
        string refreshToken,
        Guid credentialId,
        Guid scopeId,
        DateTime expireDate)
    {
        var credential = await _credentialRepository.FindAsync(credentialId);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        var scope = await _scopeRepository.FindAsync(scopeId);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        var session = new Session(token,
            refreshToken,
            credential,
            scope,
            credential.User,
            expireDate);
        await _sessionRepository.AddAsync(session);
        return session;
    }
}