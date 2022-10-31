using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Users.Exception;

namespace Managements.Domain.Users;

public interface IUserFactory
{
    Task<User> CreateAsync(Username username, GroupId groupId);
}

internal sealed class UserFactory : IUserFactory
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public UserFactory(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task<User> CreateAsync(Username username, GroupId groupId)
    {
        var exists = await _userRepository.ExistsAsync(username);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(username);
        }

        var group = await _groupRepository.FindAsync(groupId);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        return new User(username, groupId);
    }
}