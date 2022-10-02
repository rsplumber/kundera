using System.Net;

namespace Kundera.Authentication;

public interface IAuthenticateService
{
    Task<Certificate> AuthenticateAsync(string uniqueIdentifier, string password,
        string? scope = null,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}