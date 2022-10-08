using System.Net;
using Auth.Domain.Credentials;

namespace Auth.Application;

public interface IAuthenticateService
{
    Task<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        Password password,
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}