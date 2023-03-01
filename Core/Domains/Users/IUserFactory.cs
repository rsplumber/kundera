﻿using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Users.Exception;

namespace Core.Domains.Users;

public interface IUserFactory
{
    Task<User> CreateAsync(string username, Guid groupId);
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

    public async Task<User> CreateAsync(string username, Guid groupId)
    {
        var currentUser = await _userRepository.FindByUsernameAsync(username);
        if (currentUser is not null)
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