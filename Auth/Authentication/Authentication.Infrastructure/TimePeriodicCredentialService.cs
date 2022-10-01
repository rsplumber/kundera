using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class TimePeriodicCredentialService : ITimePeriodicCredentialService
{
    private readonly ICredentialRepository _credentialRepository;

    public TimePeriodicCredentialService(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, int expirationTimeInSeconds, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await Credential.CreateAsync(uniqueIdentifier,
            password,
            userId,
            expirationTimeInSeconds,
            ipAddress,
            _credentialRepository);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }
}