using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Credentials;

namespace Auth.Application.Authentication;

public interface IAuthenticateService
{
    ValueTask<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        string password,
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}