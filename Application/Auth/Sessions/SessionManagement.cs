using Application.Auth.Certificates;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Core.Domains.Users.Types;
using Core.Services;
using Mediator;
using Microsoft.Extensions.Options;

namespace Application.Auth.Sessions;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionOptions _sessionOptions;
    private readonly IMediator _mediator;

    public SessionManagement(ISessionRepository sessionRepository,
        IOptions<SessionOptions> sessionOptions,
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
            UserId = credential.User.Value,
            ScopeId = scope.Id.Value
        }, cancellationToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(_sessionOptions.ExpireInMinutes);
        if (credential.SessionExpireTimeInMinutes is not null)
        {
            expiresAt = DateTime.UtcNow.AddMinutes(credential.SessionExpireTimeInMinutes.Value);
        }

        if (credential.SingleSession)
        {
            var currentSession = await _sessionRepository.FindAsync(credential.Id, cancellationToken);
            if (currentSession is not null)
            {
                await _sessionRepository.DeleteAsync(currentSession.Id, cancellationToken);
            }
        }

        await _sessionFactory.CreateAsync(
            Token.From(certificate.Token),
            Token.From(certificate.RefreshToken),
            credential.Id,
            scope.Id,
            credential.User,
            expiresAt);

        return certificate;
    }

    public async Task DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<Session?> GetAsync(Token token, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(token, cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(userId, cancellationToken);
    }
}