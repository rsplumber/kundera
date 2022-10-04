using System.Net;

namespace Kundera.Authentication;

internal sealed class CredentialService : ICredentialService
{
    public async Task CreateAsync(string uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task CreateOneTimeAsync(string uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress, int expirationTimeInSeconds = 0, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task CreateTimePeriodicAsync(string uniqueIdentifier, string password, Guid userId, int expirationTimeInSeconds, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}