﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record DeleteUserGroupCommand(UserGroupId UserGroupId) : Command;

internal sealed class DeleteUserGroupCommandHandler : ICommandHandler<DeleteUserGroupCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public DeleteUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async Task HandleAsync(DeleteUserGroupCommand message, CancellationToken cancellationToken = default)
    {
        var userGroup = await _userGroupRepository.FindAsync(message.UserGroupId, cancellationToken);
        if (userGroup is null)
        {
            throw new UserGroupNotFoundException();
        }

        await _userGroupRepository.DeleteAsync(userGroup.Id, cancellationToken);
    }
}