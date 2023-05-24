using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Core.Hashing;

namespace Application.Auth.Sessions;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionRepository _sessionRepository;
    private readonly IHashService _hashService;

    public SessionManagement(ISessionRepository sessionRepository, ISessionFactory sessionFactory, IHashService hashService)
    {
        _sessionRepository = sessionRepository;
        _sessionFactory = sessionFactory;
        _hashService = hashService;
    }

    public async Task<Certificate> SaveAsync(Credential credential, Scope scope,CancellationToken cancellationToken = default)
    {
        var certificate = Certificate.Create(_hashService, credential, scope.Id);
        await _sessionFactory.CreateAsync(
            certificate.Token,
            certificate.RefreshToken,
            credential.Id,
            scope.Id,
            certificate.ExpireAtUtc);
        return certificate;
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<Session?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(token, cancellationToken);
    }

    public async Task<Session?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindByRefreshTokenAsync(token, cancellationToken);
    }
}