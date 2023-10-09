using Core.Auth.Authorizations;
using Core.Auth.Credentials.Exceptions;
using Core.Auth.Sessions;
using Core.Hashing;
using Core.Scopes;
using DotNetCore.CAP;

namespace Core.Auth.Credentials.Handlers;

public class AuthenticateHandler : IAuthenticateHandler
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly ICapPublisher _eventBus;
    private readonly IHashService _hashService;

    public AuthenticateHandler(ISessionManagement sessionManagement, IScopeRepository scopeRepository, ICredentialRepository credentialRepository, ICapPublisher eventBus, IHashService hashService)
    {
        _sessionManagement = sessionManagement;
        _scopeRepository = scopeRepository;
        _credentialRepository = credentialRepository;
        _eventBus = eventBus;
        _hashService = hashService;
    }

    public virtual async Task<Certificate> AuthenticateAsync(string username, string password, string scopeSecret, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default)
    {
        var scope = await _scopeRepository.FindBySecretAsync(scopeSecret, cancellationToken);
        if (scope is null) throw new InvalidScopeException();

        var credentials = await _credentialRepository.FindByUsernameAsync(username, cancellationToken);
        var credential = credentials.FirstOrDefault(credential => credential.Username == username && credential.Password.Check(password));
        if (credential is null) throw new WrongUsernamePasswordException();

        if (credential.Expired())
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
            throw new WrongUsernamePasswordException();
        }

        var certificate = await _sessionManagement.SaveAsync(credential, scope, cancellationToken);

        if (credential.OneTime)
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
        }

        _ = _eventBus.PublishAsync(AuthenticatedEvent.EventName, new AuthenticatedEvent
        {
            Username = credential.Username,
            CredentialId = credential.Id,
            UserId = credential.User.Id,
            ScopeId = scope.Id,
            Agent = requestInfo?.UserAgent,
            IpAddress = requestInfo?.IpAddress?.ToString()
        }, cancellationToken: cancellationToken);

        return certificate;
    }

    public virtual async Task<Certificate> RefreshAsync(Certificate certificate, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(certificate.Token, cancellationToken);
        var hashedRefreshToken = await _hashService.HashAsync(Session.StaticHashKey, certificate.RefreshToken);
        if (session is null || session.RefreshToken != hashedRefreshToken || RefreshTokenExpired()) throw new RefreshTokenExpiredException();

        await _sessionManagement.DeleteAsync(certificate.Token, cancellationToken);
        var refreshedCertificate = await _sessionManagement.SaveAsync(session.Credential, session.Scope, cancellationToken);

        _ = _eventBus.PublishAsync(AuthenticatedEvent.EventName, new AuthenticatedEvent
        {
            Agent = requestInfo?.UserAgent,
            IpAddress = requestInfo?.IpAddress?.ToString(),
            Username = session.Credential.Username,
            CredentialId = session.Credential.Id,
            UserId = session.User.Id,
            ScopeId = session.Scope.Id
        }, cancellationToken: cancellationToken);

        return refreshedCertificate;

        bool RefreshTokenExpired() => DateTime.UtcNow >= session.RefreshTokenExpirationDateUtc;
    }

    public virtual async Task LogoutAsync(Certificate certificate, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(certificate.Token, cancellationToken);
        var hashedRefreshToken = await _hashService.HashAsync(Session.StaticHashKey, certificate.RefreshToken);
        if (session is null || session.RefreshToken != hashedRefreshToken) throw new SessionNotFoundException();

        await _sessionManagement.DeleteAsync(session.Id, cancellationToken);
    }
}