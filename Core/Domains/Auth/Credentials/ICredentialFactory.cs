using System.Net;
using Core.Domains.Auth.Credentials.Exceptions;
using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Credentials;

public interface ICredentialFactory
{
    Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        UserId user,
        IPAddress? ipAddress = null,
        int expireInMinutes = 0,
        bool oneTime = false);
}

internal sealed class CredentialFactory : ICredentialFactory
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;

    public CredentialFactory(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier, string password, UserId userId, IPAddress? ipAddress = null, int expireInMinutes = 0, bool oneTime = false)
    {
        var currentCredential = await _credentialRepository.FindAsync(uniqueIdentifier);
        if (currentCredential is not null)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        var user = await _userRepository.FindAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var requestedUsername = Username.From(uniqueIdentifier.Username);
        if (!user.Has(requestedUsername))
        {
            throw new UsernameNotFoundException();
        }

        var credential = new Credential(uniqueIdentifier, password, userId, oneTime, expireInMinutes, ipAddress);
        await _credentialRepository.AddAsync(credential);
        return credential;
    }
}