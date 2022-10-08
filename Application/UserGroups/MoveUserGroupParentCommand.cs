﻿using Domain.UserGroups;
using Domain.UserGroups.Exception;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId UserGroupId, UserGroupId To) : Command;

internal sealed class MoveUserGroupParentCommandHandler : CommandHandler<MoveUserGroupParentCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public MoveUserGroupParentCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(MoveUserGroupParentCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, to) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.SetParent(to);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}