using System.Net;
using Auth.Core.Entities;

namespace Auth.Core.Services;

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