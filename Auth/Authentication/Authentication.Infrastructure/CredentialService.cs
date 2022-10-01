using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialRepository _credentialRepository;

    public CredentialService(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await Credential.CreateAsync(uniqueIdentifier, password, userId, ipAddress, _credentialRepository);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }

    public async Task UpdateAsync(UniqueIdentifier uniqueIdentifier, Password newPassword, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null) return;
        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
    }

    public async Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
    }
}