using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using Core.Domains.Users.Exception;

namespace Core.Domains.Users;

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

        var user = new User(username, groupId);
        await _userRepository.AddAsync(user);
        return user;
    }
}