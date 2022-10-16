using System.Net;
using Auth.Application.Authentication;
using Auth.Application.Authorization;
using Auth.Domain.Credentials;
using Auth.Domain.Sessions;

namespace Authentication.Infrastructure.Authentication;

internal class AuthenticateService : IAuthenticateService
{
    private readonly ICredentialService _credentialService;
    private readonly ICertificateService _certificateService;
    private readonly ISessionManagement _sessionManagement;

    public AuthenticateService(ICertificateService certificateService,
        ISessionManagement sessionManagement,
        ICredentialService credentialService)
    {
        _certificateService = certificateService;
        _sessionManagement = sessionManagement;
        _credentialService = credentialService;
    }

    public async ValueTask<Certificate> AuthenticateAsync(UniqueIdentifier uniqueIdentifier, string password, string scope = "global", IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialService.FindAsync(uniqueIdentifier, ipAddress, cancellationToken);
        if (credential is null)
        {
            throw new UnAuthenticateException();
        }

        credential.Password.Check(password);

        var certificate = await _certificateService.GenerateAsync(credential.User.ToString(), scope, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, credential.User, scope, ipAddress ?? IPAddress.None, cancellationToken);

        return certificate;
    }

    public async ValueTask<Certificate> RefreshCertificateAsync(Token token, Token refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
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
        var scope = session.Scope;
        var certificate = await _certificateService.GenerateAsync(userId.ToString(), scope, cancellationToken);
        await _sessionManagement.SaveAsync(certificate, userId, scope, ipAddress ?? IPAddress.None, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }
}