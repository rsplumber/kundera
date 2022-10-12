using System.Net;

namespace Kundera.Authentication;

public interface IAuthenticateService
{
    Task<Certificate?> AuthenticateAsync(string username,
        string password,
        string type = "default",
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    ValueTask<Certificate?> RefreshCertificateAsync(string token, string refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default);
}