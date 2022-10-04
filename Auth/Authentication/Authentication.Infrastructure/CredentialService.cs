﻿using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Types;
using Users.Domain.Users;
using Users.Domain.Users.Exception;

namespace Authentication.Infrastructure;

internal class CredentialService : ICredentialService
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;

    public CredentialService(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, Guid userId, Password password, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, cancellationToken: cancellationToken);
    }

    public async Task CreateOneTimeAsync(UniqueIdentifier uniqueIdentifier, Guid userId, Password password, int expirationTimeInSeconds = 0, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await ValidateUser(uniqueIdentifier, userId, cancellationToken);
        await CreateCredential(uniqueIdentifier, password, userId, ipAddress, expirationTimeInSeconds, true, cancellationToken);
    }

    public async Task CreateTimePeriodicAsync(UniqueIdentifier uniqueIdentifier, Guid userId, Password password, int expirationTimeInSeconds, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
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

    private async Task CreateCredential(UniqueIdentifier uniqueIdentifier,
        Password password,
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
            throw new UserNotFoundException();
        }
    }
}