using System.Net;
using Auth.Domain.Credentials;

namespace Auth.Application.Authentication;

public interface ICredentialService
{
    ValueTask CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);


    ValueTask CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, 
        string password,
        Guid userId,
        int expirationTimeInSeconds = 0,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    ValueTask CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, 
        string password,
        Guid userId,
        int expirationTimeInSeconds,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    ValueTask ChangePasswordAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        string newPassword,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    ValueTask RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}