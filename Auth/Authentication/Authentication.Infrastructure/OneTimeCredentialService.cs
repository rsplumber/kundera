using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class OneTimeCredentialService : IOneTimeCredentialService
{
    private readonly ICredentialRepository _credentialRepository;

    public OneTimeCredentialService(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, int expirationTimeInSeconds = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var credential = await Credential.CreateAsync(uniqueIdentifier,
            password,
            userId,
            true,
            expirationTimeInSeconds,
            ipAddress,
            _credentialRepository);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }
}