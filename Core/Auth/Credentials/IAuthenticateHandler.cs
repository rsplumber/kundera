using System.Net;
using Core.Auth.Authorizations;
using Core.Auth.Credentials.Exceptions;
using Core.Auth.Sessions;
using Core.Scopes;
using DotNetCore.CAP;

namespace Core.Auth.Credentials;

public interface IAuthenticateHandler
{
    Task<Certificate> AuthenticateAsync(string username, string password, string scopeSecret, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default);

    Task<Certificate> RefreshAsync(Certificate certificate, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default);

    Task LogoutAsync(Certificate certificate, CancellationToken cancellationToken = default);
}

public sealed record RequestInfo
{
    public string? UserAgent { get; init; } = default;

    public IPAddress? IpAddress { get; init; } = default;
}

internal class AuthenticateHandler : IAuthenticateHandler
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ICapPublisher _eventBus;

    public AuthenticateHandler(ISessionManagement sessionManagement, IScopeRepository scopeRepository, ICredentialRepository credentialRepository, ISessionRepository sessionRepository, ICapPublisher eventBus)
    {
        _sessionManagement = sessionManagement;
        _scopeRepository = scopeRepository;
        _credentialRepository = credentialRepository;
        _sessionRepository = sessionRepository;
        _eventBus = eventBus;
    }

    public async Task<Certificate> AuthenticateAsync(string username, string password, string scopeSecret, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default)
    {
        var scope = await _scopeRepository.FindBySecretAsync(scopeSecret, cancellationToken);
        if (scope is null) throw new InvalidScopeException();

        var credentials = await _credentialRepository.FindByUsernameAsync(username, cancellationToken);
        var credential = credentials.FirstOrDefault(credential => credential.Username == username && credential.Password.Check(password));
        if (credential is null) throw new WrongUsernamePasswordException();

        if (CredentialExpired())
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
            throw new WrongUsernamePasswordException();
        }

        var certificate = await _sessionManagement.SaveAsync(credential, scope, cancellationToken);

        if (credential.SingleSession)
        {
            await RemoveOtherSessionsAsync();
        }

        if (credential.OneTime)
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
        }

        await Task.Run(() =>
        {
            _eventBus.PublishAsync(AuthenticatedEvent.EventName, new AuthenticatedEvent
            {
                Agent = requestInfo?.UserAgent,
                IpAddress = requestInfo?.IpAddress?.ToString(),
                Username = credential.Username,
                CredentialId = credential.Id,
                UserId = credential.User.Id,
                ScopeId = scope.Id
            }, cancellationToken: cancellationToken);
        }, cancellationToken);

        return certificate;

        bool CredentialExpired() => DateTime.UtcNow >= credential.ExpiresAtUtc;

        async Task RemoveOtherSessionsAsync()
        {
            var currentCredentialSessions = await _sessionRepository.FindByCredentialIdAsync(credential.Id, cancellationToken);
            foreach (var currentCredentialSession in currentCredentialSessions.Where(session => session.Id != certificate.Token))
            {
                await _sessionRepository.DeleteAsync(currentCredentialSession.Id, cancellationToken);
            }
        }
    }

    public async Task<Certificate> RefreshAsync(Certificate certificate, RequestInfo? requestInfo = null, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(certificate.Token, cancellationToken);
        if (session is null || session.RefreshToken != certificate.RefreshToken || RefreshTokenExpired())
        {
            throw new RefreshTokenExpiredException();
        }

        await _sessionManagement.DeleteAsync(certificate.Token, cancellationToken);
        await Task.Run(() =>
        {
            _eventBus.PublishAsync(AuthenticatedEvent.EventName, new AuthenticatedEvent
            {
                Agent = requestInfo?.UserAgent,
                IpAddress = requestInfo?.IpAddress?.ToString(),
                Username = session.Credential.Username,
                CredentialId = session.Credential.Id,
                UserId = session.User.Id,
                ScopeId = session.Scope.Id
            }, cancellationToken: cancellationToken);
        }, cancellationToken);

        return await _sessionManagement.SaveAsync(session.Credential, session.Scope, cancellationToken);

        bool RefreshTokenExpired() => DateTime.UtcNow >= session.RefreshTokenExpirationDateUtc;
    }

    public async Task LogoutAsync(Certificate certificate, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(certificate.Token, cancellationToken);
        if (session is null || certificate.RefreshToken != session.RefreshToken)
        {
            throw new SessionNotFoundException();
        }

        await _sessionManagement.DeleteAsync(session.Id, cancellationToken);
    }
}