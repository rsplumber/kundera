﻿using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Users;
using Core.Domains.Users.Exception;
using Mediator;

namespace Application.Users;

public sealed record RemoveUserFromGroupCommand : ICommand
{
    public Guid UserId { get; init; } = default!;

    public Guid GroupId { get; init; } = default!;
}

internal sealed class RemoveUserFromGroupCommandHandler : ICommandHandler<RemoveUserFromGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public RemoveUserFromGroupCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(RemoveUserFromGroupCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        user.Leave(group);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}