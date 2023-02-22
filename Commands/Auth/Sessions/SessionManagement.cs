using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Mediator;
using Microsoft.Extensions.Options;

namespace Commands.Auth.Sessions;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionDefaultOptions _sessionOptions;
    private readonly IMediator _mediator;

    public SessionManagement(ISessionRepository sessionRepository,
        IOptions<SessionDefaultOptions> sessionOptions,
        ISessionFactory sessionFactory,
        IMediator mediator)
    {
        _sessionRepository = sessionRepository;
        _sessionFactory = sessionFactory;
        _mediator = mediator;
        _sessionOptions = sessionOptions.Value;
    }

    public async Task<Certificate> SaveAsync(Credential credential, Scope scope, CancellationToken cancellationToken = default)
    {
        var certificate = await _mediator.Send(new GenerateCertificateCommand
        {
            UserId = credential.UserId,
            ScopeId = scope.Id
        }, cancellationToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(_sessionOptions.ExpireInMinutes);
        if (credential.SessionExpireTimeInMinutes is not null)
        {
            expiresAt = DateTime.UtcNow.AddMinutes(credential.SessionExpireTimeInMinutes.Value);
        }

        if (credential.SingleSession)
        {
            var currentSession = await _sessionRepository.FindByCredentialIdAsync(credential.Id, cancellationToken);
            if (currentSession is not null)
            {
                await _sessionRepository.DeleteAsync(currentSession.Id, cancellationToken);
            }
        }

        await _sessionFactory.CreateAsync(
            certificate.Token,
            certificate.RefreshToken,
            credential.Id,
            scope.Id,
            credential.UserId,
            expiresAt);

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

    public async Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindByUserIdAsync(userId, cancellationToken);
    }
}