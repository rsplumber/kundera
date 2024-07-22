using Core.Auth.Credentials.Exceptions;
using Core.Users;
using Core.Users.Exception;

namespace Core.Auth.Credentials;

public interface ICredentialFactory
{
    Task<Credential> CreateAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        bool? singleSession = false);

    Task<Credential> CreateOneTimeAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        int expireInMinutes = 0);

    Task<Credential> CreateTimePeriodicAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        int expireInMinutes,
        bool? singleSession = false);
}

internal sealed class CredentialFactory : ICredentialFactory
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUserRepository _userRepository;
    private const int DefaultSessionExpireTimeInMinutes = 15;

    public CredentialFactory(ICredentialRepository credentialRepository, IUserRepository userRepository)
    {
        _credentialRepository = credentialRepository;
        _userRepository = userRepository;
    }

    public async Task<Credential> CreateAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        bool? singleSession = false)
    {
        var user = await ValidateCredentialAsync(userId, username, password);
        var credential = new Credential(username, password, user)
        {
            SingleSession = singleSession ?? false,
            SessionTokenExpireTimeInMinutes = sessionTokenExpireTimeInMinutes > 0 ? sessionTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes,
            SessionRefreshTokenExpireTimeInMinutes = sessionRefreshTokenExpireTimeInMinutes > 0 ? sessionRefreshTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    public async Task<Credential> CreateOneTimeAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        int expireInMinutes = 0)
    {
        var user = await _userRepository.FindAsync(userId);
        if (user is null) throw new UserNotFoundException();

        var currentCredentials = await _credentialRepository.FindByUsernameAsync(username);
        var selectedCredential = currentCredentials.FirstOrDefault(credential => credential.Username == username && credential.Password.Check(password));
        if (selectedCredential is not null)
        {
            await _credentialRepository.DeleteAsync(selectedCredential.Id);
        }
        
        var credential = new Credential(username, password, user, true, expireInMinutes)
        {
            SingleSession = true,
            SessionTokenExpireTimeInMinutes = sessionTokenExpireTimeInMinutes > 0 ? sessionTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes,
            SessionRefreshTokenExpireTimeInMinutes = sessionRefreshTokenExpireTimeInMinutes > 0 ? sessionRefreshTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes,
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    public async Task<Credential> CreateTimePeriodicAsync(
        Guid userId,
        string username,
        string password,
        int sessionTokenExpireTimeInMinutes,
        int sessionRefreshTokenExpireTimeInMinutes,
        int expireInMinutes,
        bool? singleSession = false)
    {
        var user = await ValidateCredentialAsync(userId, username, password);
        var credential = new Credential(username, password, user, expireInMinutes)
        {
            SingleSession = singleSession ?? false,
            SessionTokenExpireTimeInMinutes = sessionTokenExpireTimeInMinutes > 0 ? sessionTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes,
            SessionRefreshTokenExpireTimeInMinutes = sessionRefreshTokenExpireTimeInMinutes > 0 ? sessionRefreshTokenExpireTimeInMinutes : DefaultSessionExpireTimeInMinutes,
        };
        await _credentialRepository.AddAsync(credential);
        return credential;
    }

    private async Task<User> ValidateCredentialAsync(Guid userId, string username, string password)
    {
        var user = await _userRepository.FindAsync(userId);
        if (user is null) throw new UserNotFoundException();

        var currentCredentials = await _credentialRepository.FindByUsernameAsync(username);
        if (currentCredentials.Any(credential => credential.Username == username && credential.Password.Check(password)))
        {
            throw new DuplicateUniqueIdentifierException(username);
        }

        return user;
    }
}