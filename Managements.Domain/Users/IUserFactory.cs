using Managements.Domain.Groups;
using Managements.Domain.Users.Exception;

namespace Managements.Domain.Users;

public interface IUserFactory
{
    Task<User> CreateAsync(Username username, GroupId groupId);
}

internal sealed class UserFactory : IUserFactory
{
    private readonly IUserRepository _userRepository;

    public UserFactory(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateAsync(Username username, GroupId groupId)
    {
        var exists = await _userRepository.ExistsAsync(username);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(username);
        }

        return new User(username, groupId);
    }
}