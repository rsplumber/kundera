using System.Net;

namespace Auth.Core.Services;

public interface IAuthenticateService
{
    Task<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        string password,
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task<Certificate> RefreshCertificateAsync(Token token,
        Token refreshToken,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}