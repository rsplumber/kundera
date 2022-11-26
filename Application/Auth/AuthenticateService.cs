﻿using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Credentials.Exceptions;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Types;
using Core.Domains.Sessions;
using Core.Services;

namespace Application.Auth;

internal class AuthenticateService : IAuthenticateService
{
    private readonly ICredentialService _credentialService;
    private readonly ICertificateService _certificateService;
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;

    public AuthenticateService(ICertificateService certificateService,
        ISessionManagement sessionManagement,
        ICredentialService credentialService,
        IScopeRepository scopeRepository)
    {
        _certificateService = certificateService;
        _sessionManagement = sessionManagement;
        _credentialService = credentialService;
        _scopeRepository = scopeRepository;
    }

    public async Task<Certificate> AuthenticateAsync(UniqueIdentifier uniqueIdentifier, string password, string scopeSecret, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialService.FindAsync(uniqueIdentifier, cancellationToken);
        var scope = await _scopeRepository.FindAsync(ScopeSecret.From(scopeSecret), cancellationToken);
        if (credential is null || scope is null)
        {
            // await PublishUnAuthenticateEvent(null, uniqueIdentifier.Value, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        try
        {
            credential.Password.Check(password);
        }
        catch
        {
            // await PublishUnAuthenticateEvent(scope.Id.Value, uniqueIdentifier.Value, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        var certificate = await _certificateService.GenerateAsync(credential.User, scope.Id, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, credential.User, scope.Id, cancellationToken);

        // await _eventBus.PublishAsync(new OnAuthenticateEvent(scopeId,
        //     uniqueIdentifier.Value,
        //     ipAddress ?? IPAddress.None), cancellationToken);

        return certificate;
    }

    public async Task<Certificate> RefreshCertificateAsync(Token token, Token refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, cancellationToken);
        if (session is null || session.RefreshToken != refreshToken)
        {
            // await PublishUnAuthenticateEvent(session?.Scope, null, ipAddress, cancellationToken);
            throw new UnAuthenticateException();
        }

        var userId = session.User;
        var scopeId = session.Scope;
        var certificate = await _certificateService.GenerateAsync(userId, scopeId, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, userId, scopeId, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }

    // private Task PublishUnAuthenticateEvent(Guid? scopeId, string? uniqueIdentifier, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    // {
    //     // return _eventBus.PublishAsync(new OnUnAuthenticateEvent
    //     // {
    //     //     ScopeId = scopeId,
    //     //     UniqueIdentifier = uniqueIdentifier,
    //     //     IpAddress = ipAddress
    //     // }, cancellationToken);
    //     return Task.CompletedTask;
    // }
}