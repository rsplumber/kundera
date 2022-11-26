using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Sessions;

namespace Core.Services;

public interface IAuthenticateService
{
    Task<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        string password,
        string scopeSecret,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task<Certificate> RefreshCertificateAsync(Token token,
        Token refreshToken,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}