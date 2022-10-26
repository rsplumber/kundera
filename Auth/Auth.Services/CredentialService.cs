using System.Net;
using Auth.Core;
using Auth.Core.Exceptions;
using Auth.Core.Services;

namespace Auth.Services;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialFactory _credentialFactory;
    private readonly ICredentialRepository _credentialRepository;

    public CredentialService(ICredentialRepository credentialRepository, ICredentialFactory credentialFactory)
    {
        _credentialRepository = credentialRepository;
        _credentialFactory = credentialFactory;
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        var credential = await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress: ipAddress);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }

    public async Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expireInMinutes = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        var credential = await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress,
            expireInMinutes);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }

    public async Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expireInMinutes, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        var credential = await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress,
            expireInMinutes,
            true);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }

    public async Task UpdateUsageAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.UpdateActivityInfo(ipAddress ?? IPAddress.None);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
    }

    public async Task ChangePasswordAsync(UniqueIdentifier uniqueIdentifier, string password, string newPassword, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.ChangePassword(password, newPassword);
        credential.UpdateActivityInfo(ipAddress ?? IPAddress.None);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
    }

    public async Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
    }

    public async Task<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress = default, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null) return null;

        if (Expired())
        {
            await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
            return null;
        }

        if (credential.OneTime)
        {
            await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
            return credential;
        }

        credential.UpdateActivityInfo(ipAddress ?? IPAddress.None);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        return credential;

        bool Expired() => DateTime.UtcNow >= credential.ExpiresAt;
    }


    private async Task ValidateUser(UniqueIdentifier uniqueIdentifier, Guid userId, CancellationToken cancellationToken)
    {
    }
}