using System.Net;
using Auth.Application.Events;
using Auth.Core;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Core.Services;
using Kite.Events;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;

namespace Auth.Application.Services;

internal class AuthenticateService : IAuthenticateService
{
    private readonly ICredentialService _credentialService;
    private readonly ICertificateService _certificateService;
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;
    private readonly IEventBus _eventBus;

    public AuthenticateService(ICertificateService certificateService,
        ISessionManagement sessionManagement,
        ICredentialService credentialService,
        IScopeRepository scopeRepository,
        IEventBus eventBus)
    {
        _certificateService = certificateService;
        _sessionManagement = sessionManagement;
        _credentialService = credentialService;
        _scopeRepository = scopeRepository;
        _eventBus = eventBus;
    }

    public async Task<Certificate> AuthenticateAsync(UniqueIdentifier uniqueIdentifier, string password, string scopeSecret, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialService.FindAsync(uniqueIdentifier, cancellationToken);
        var scope = await _scopeRepository.FindAsync(ScopeSecret.From(scopeSecret), cancellationToken);
        if (credential is null || scope is null)
        {
            await PublishUnAuthenticateEvent(null, uniqueIdentifier.Value, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        try
        {
            credential.Password.Check(password);
        }
        catch
        {
            await PublishUnAuthenticateEvent(scope.Id.Value, uniqueIdentifier.Value, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        var scopeId = scope.Id.Value;
        var certificate = await _certificateService.GenerateAsync(credential.UserId, scopeId, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, credential.UserId, scopeId, cancellationToken);

        await _eventBus.PublishAsync(new OnAuthenticateEvent(scopeId,
            uniqueIdentifier.Value,
            ipAddress ?? IPAddress.None), cancellationToken);

        return certificate;
    }

    public async Task<Certificate> RefreshCertificateAsync(Token token, Token refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, cancellationToken);
        if (session is null || session.RefreshToken != refreshToken)
        {
            await PublishUnAuthenticateEvent(session?.ScopeId, null, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        var userId = session.UserId;
        var scopeId = session.ScopeId;
        var certificate = await _certificateService.GenerateAsync(userId, scopeId, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, userId, scopeId, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }

    private Task PublishUnAuthenticateEvent(Guid? scopeId, string? uniqueIdentifier, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        return _eventBus.PublishAsync(new OnUnAuthenticateEvent
        {
            ScopeId = scopeId,
            UniqueIdentifier = uniqueIdentifier,
            IpAddress = ipAddress
        }, cancellationToken);
    }
}