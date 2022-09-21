﻿using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupNameCommand(UserGroupId UserGroup, Name Name) : Command;

internal sealed class ChangeUserGroupNameCommandHandler : CommandHandler<ChangeUserGroupNameCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public ChangeUserGroupNameCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(ChangeUserGroupNameCommand message, CancellationToken cancellationToken = default)
    {
        var group = await _userGroupRepository.FindAsync(message.UserGroup, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        group.ChangeName(message.Name);
        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}