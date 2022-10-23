﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record SetUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;

internal sealed class SetUserGroupParentCommandHandler : ICommandHandler<SetUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public SetUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(SetUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, parentId) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        var parent = await _userGroupRepository.FindAsync(parentId, cancellationToken);
        if (parent is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}