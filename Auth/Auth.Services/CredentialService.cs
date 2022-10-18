﻿using System.Net;
using Auth.Core;
using Auth.Core.Exceptions;
using Auth.Core.Services;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;

namespace Auth.Services;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;

    public CredentialService(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, cancellationToken: cancellationToken);
    }

    public async Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expirationTimeInSeconds = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, expirationTimeInSeconds, true, cancellationToken);
    }

    public async Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, int expirationTimeInSeconds, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, expirationTimeInSeconds, cancellationToken: cancellationToken);
    }

    public async Task UpdateUsageAsync(UniqueIdentifier uniqueIdentifier, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier, cancellationToken);
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.UpdateActivityInfo(ipAddress);
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
        credential.UpdateActivityInfo(ipAddress);
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

        credential.UpdateActivityInfo(ipAddress);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        return credential;

        bool Expired() => DateTime.UtcNow >= credential.ExpiresAt;
    }

    private async Task CreateCredential(UniqueIdentifier uniqueIdentifier,
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

    private async Task ValidateUser(UniqueIdentifier uniqueIdentifier, Guid userId, CancellationToken cancellationToken)
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