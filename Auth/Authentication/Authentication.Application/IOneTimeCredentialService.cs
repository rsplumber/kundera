using System.Net;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Application;

public interface IOneTimeCredentialService : ICredentialRemoveService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier,
        UserId userId,
        Password password,
        int expirationTimeInSeconds = 0,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}