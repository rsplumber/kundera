using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Credentials;
using Auth.Domain.Sessions;

namespace Auth.Application.Authentication;

public interface IAuthenticateService
{
    ValueTask<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        string password,
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    ValueTask<Certificate> RefreshCertificateAsync(Token token,
        Token refreshToken,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}