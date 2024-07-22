using Core.Groups;
using Core.Groups.Exception;

namespace Core.Users;

public interface IUserFactory
{
    Task<User> CreateAsync(Guid groupId);

    Task<User> CreateAsync(Guid userId, Guid groupId);
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

    public async Task<User> CreateAsync(Guid groupId)
    {
        var group = await _groupRepository.FindAsync(groupId);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var user = new User(group);
        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User> CreateAsync(Guid userId, Guid groupId)
    {
        var group = await _groupRepository.FindAsync(groupId);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var user = new User(userId, group);
        await _userRepository.AddAsync(user);
        return user;
    }
}