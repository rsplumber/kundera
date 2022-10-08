﻿using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class EnableUserGroupCommandHandler : CommandHandler<EnableUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public EnableUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(EnableUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.Enable();
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}