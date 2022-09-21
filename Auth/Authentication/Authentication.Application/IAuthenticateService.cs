using System.Net;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Application;

public interface IAuthenticateService
{
    Task<OneTimeToken> AuthenticateAsync(UniqueIdentifier uniqueIdentifier,
        Password password,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}