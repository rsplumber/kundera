using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Users.Types;

namespace Core.Services;

public interface ICredentialService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        UserId userId,
        IPAddress? ipAddress,
        CancellationToken cancellationToken = default);


    Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        UserId userId,
        int expireInMinutes = 0,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        UserId userId,
        int expireInMinutes,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        string newPassword,
        IPAddress? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}