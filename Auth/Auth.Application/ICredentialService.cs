using System.Net;
using Auth.Domain.Credentials;

namespace Auth.Application;

public interface ICredentialService : ICredentialRemoveService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier,
        Guid userId,
        Password password,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);


    Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier,
        Guid userId,
        Password password,
        int expirationTimeInSeconds = 0,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier,
        Guid userId,
        Password password,
        int expirationTimeInSeconds,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        string newPassword,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);
}