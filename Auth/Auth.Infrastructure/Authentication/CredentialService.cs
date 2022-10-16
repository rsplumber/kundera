using System.Net;
using Auth.Application.Authentication;
using Auth.Domain.Credentials;
using Auth.Domain.Credentials.Exceptions;
using Domain.Users;
using Domain.Users.Exception;

namespace Authentication.Infrastructure.Authentication;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;

    public CredentialService(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async ValueTask CreateAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, cancellationToken: cancellationToken);
    }

    public async ValueTask CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expirationTimeInSeconds = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, expirationTimeInSeconds, true, cancellationToken);
    }

    public async ValueTask CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expirationTimeInSeconds, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, expirationTimeInSeconds, cancellationToken: cancellationToken);
    }

    public async ValueTask UpdateUsageAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
    }

    public async ValueTask ChangePasswordAsync(UniqueIdentifier uniqueIdentifier, string password, string newPassword, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.ChangePassword(password, newPassword);
        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);
    }

    public async ValueTask RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        await _credentialRepository.DeleteAsync(uniqueIdentifier, cancellationToken);
    }
    
    public async ValueTask<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress = default, CancellationToken cancellationToken = default)
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

        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        return credential;

        bool Expired() => DateTime.UtcNow >= credential.ExpiresAt;
    }

    private async ValueTask CreateCredential(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress = null,
        int expirationTimeInSeconds = 0,
        bool oneTime = false,
        CancellationToken cancellationToken = default)
    {
        var credential = await Credential.CreateAsync(uniqueIdentifier,
            password,
            userId,
            oneTime,
            expirationTimeInSeconds,
            ipAddress,
            _credentialRepository);
        await _credentialRepository.AddAsync(credential, cancellationToken);
    }

    private async ValueTask ValidateUser(UniqueIdentifier uniqueIdentifier, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(userId), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var requestedUsername = Username.From(uniqueIdentifier.Username);
        if (!user.Has(requestedUsername))
        {
            throw new UsernameNotFoundException();
        }
    }
}