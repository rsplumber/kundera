using System.Net;
using Auth.Core.Exceptions;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;

namespace Auth.Core;

public interface ICredentialFactory
{
    Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
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

    public async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? ipAddress = null, int expireInMinutes = 0, bool oneTime = false)
    {
        var exists = await _credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        var user = await _userRepository.FindAsync(UserId.From(userId));
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