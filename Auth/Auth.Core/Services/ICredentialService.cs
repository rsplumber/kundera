using System.Net;

namespace Auth.Core.Services;

public interface ICredentialService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);


    Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        int expirationTimeInSeconds = 0,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        int expirationTimeInSeconds,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        string newPassword,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress = default, CancellationToken cancellationToken = default);
}