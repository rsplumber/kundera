using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Hashing;
using Core.Scopes;

namespace Core.Auth.Sessions;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IHashService _hashService;

    public SessionManagement(ISessionRepository sessionRepository, IHashService hashService)
    {
        _sessionRepository = sessionRepository;
        _hashService = hashService;
    }


    public async Task<Certificate> SaveAsync(Credential credential, Scope scope, CancellationToken cancellationToken = default)
    {
        var certificate = Certificate.Create(_hashService, credential, scope.Id);
        var session = new Session(_hashService, certificate, credential, scope);
        if (credential.SingleSession)
        {
            await RemoveOtherSessionsAsync();
        }

        await _sessionRepository.AddAsync(session, cancellationToken);
        return certificate;

        async Task RemoveOtherSessionsAsync()
        {
            var currentCredentialSessions = await _sessionRepository.FindByCredentialIdAsync(credential.Id, cancellationToken);
            foreach (var currentCredentialSession in currentCredentialSessions)
            {
                await _sessionRepository.DeleteAsync(currentCredentialSession.Id, cancellationToken);
            }
        }
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<Session?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        var hashedToken = await _hashService.HashAsync(Session.StaticHashKey, token);
        return await _sessionRepository.FindAsync(hashedToken, cancellationToken);
    }
}