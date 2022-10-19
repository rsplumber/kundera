using System.Net;
using Auth.Core;
using Auth.Core.Exceptions;
using Auth.Core.Services;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;

namespace Auth.Services;

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
        var credential = await _credentialService.FindAsync(uniqueIdentifier, ipAddress, cancellationToken);
        if (credential is null)
        {
            throw new UnAuthenticateException();
        }

        var scope = await _scopeRepository.FindAsync(ScopeSecret.From(scopeSecret), cancellationToken);
        if (scope is null)
        {
            throw new UnAuthenticateException();
        }


        credential.Password.Check(password);


        var certificate = await _certificateService.GenerateAsync(credential.UserId, scope.Id.Value, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, credential.UserId, scope.Id.Value, ipAddress ?? IPAddress.None, cancellationToken);

        return certificate;
    }

    public async Task<Certificate> RefreshCertificateAsync(Token token, Token refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var session = await _sessionManagement.GetAsync(token, ipAddress ?? IPAddress.None, cancellationToken);
        if (session is null)
        {
            throw new UnAuthenticateException();
        }

        if (session.RefreshToken != refreshToken)
        {
            throw new UnAuthenticateException();
        }

        var userId = session.UserId;
        var scopeId = session.ScopeId;
        var certificate = await _certificateService.GenerateAsync(userId, scopeId, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, userId, scopeId, ipAddress ?? IPAddress.None, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }
}