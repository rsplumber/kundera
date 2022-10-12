using System.Net;

namespace Kundera.Authentication;

public interface ICredentialService
{
    Task<bool> CreateAsync(string username, string password, Guid userId, string type,
        CancellationToken cancellationToken = default);

    Task<bool> CreateOneTimeAsync(string username, string password, Guid userId, string type, int expirationTimeInSeconds = 0,
        CancellationToken cancellationToken = default);

    Task<bool> CreateTimePeriodicAsync(string username, string password, Guid userId, string type, int expirationTimeInSeconds,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string uniqueIdentifier, CancellationToken cancellationToken = default);
}