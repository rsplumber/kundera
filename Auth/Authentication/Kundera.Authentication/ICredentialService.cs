using System.Net;

namespace Kundera.Authentication;

public interface ICredentialService
{
    Task CreateAsync(string uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);

    Task CreateOneTimeAsync(string uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress,
        int expirationTimeInSeconds = 0,
        CancellationToken cancellationToken = default);

    Task CreateTimePeriodicAsync(string uniqueIdentifier,
        string password,
        Guid userId,
        int expirationTimeInSeconds,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string uniqueIdentifier, CancellationToken cancellationToken = default);
}