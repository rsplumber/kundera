using System.Net;
using Authentication.Domain;
using Authentication.Domain.Types;
using Authorization.Application;

namespace Authentication.Application;

public interface IAuthenticateService
{
    Task<Certificate> AuthenticateAsync(
        UniqueIdentifier uniqueIdentifier,
        Password password,
        string scope = "global",
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}