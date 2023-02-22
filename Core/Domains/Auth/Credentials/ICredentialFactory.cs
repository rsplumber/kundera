using Core.Domains.Auth.Credentials.Exceptions;
using Core.Domains.Users;
using Core.Domains.Users.Exception;

namespace Core.Domains.Auth.Credentials;

public interface ICredentialFactory
{
    Task<Credential> CreateAsync(string username,
        string password,
        Guid userId,
        bool? singleSession = false);

    Task<Credential> CreateOneTimeAsync(string username,
        string password,
        Guid userId,
        int expireInMinutes = 0);

    Task<Credential> CreateTimePeriodicAsync(string username,
        string password,
        Guid userId,
        int expireInMinutes,
        bool? singleSession = false);
}

public sealed class CredentialFactory : ICredentialFactory
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;

    public CredentialFactory(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async Task<Credential> CreateAsync(string username, string password, Guid user, bool? singleSession = false)
    {
        await ValidateAsync(username, password, user);
        var credential = new Credential(username, password, user)
        {
            SingleSession = singleSession ?? false
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    public async Task<Credential> CreateOneTimeAsync(string username, string password, Guid user, int expireInMinutes = 0)
    {
        await ValidateAsync(username, password, user);
        var credential = new Credential(username, password, user, true, expireInMinutes)
        {
            SingleSession = true
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    public async Task<Credential> CreateTimePeriodicAsync(string username, string password, Guid user, int expireInMinutes, bool? singleSession = false)
    {
        await ValidateAsync(username, password, user);
        var credential = new Credential(username, password, user, expireInMinutes)
        {
            SingleSession = singleSession ?? false
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    private async Task ValidateAsync(string username, string password, Guid userId)
    {
        var user = await _userRepository.FindAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        if (!user.HasUsername(username))
        {
            throw new UsernameNotFoundException();
        }

        var currentCredentials = await _credentialRepository.FindByUsernameAsync(username);
        if (currentCredentials.Any(credential => credential.Username == username && credential.Password.Check(password)))
        {
            throw new DuplicateUniqueIdentifierException(username);
        }
    }
}