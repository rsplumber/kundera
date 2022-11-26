using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Credentials.Exceptions;
using Core.Domains.Users.Types;
using Core.Services;

namespace Application.Auth;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialFactory _credentialFactory;
    private readonly ICredentialRepository _credentialRepository;

    public CredentialService(ICredentialRepository credentialRepository, ICredentialFactory credentialFactory)
    {
        _credentialRepository = credentialRepository;
        _credentialFactory = credentialFactory;
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, string password, UserId userId, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress);
    }

    public async Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, string password, UserId userId, int expireInMinutes = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress,
            expireInMinutes);
    }

    public async Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, string password, UserId userId, int expireInMinutes, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await _credentialFactory.CreateAsync(uniqueIdentifier,
            password,
            userId,
            ipAddress,
            expireInMinutes,
            true);
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

    public async Task<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null) return null;

        if (Expired())
        {
            await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
            return null;
        }

        if (!credential.OneTime) return credential;
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
        return credential;

        bool Expired() => DateTime.UtcNow >= credential.ExpiresAt;
    }
}