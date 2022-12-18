﻿using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Groups.Types;
using Mediator;

namespace Application.Groups;

public sealed record ChangeGroupInfoCommand : ICommand
{
    public Guid Group { get; init; } = default!;

    public string Name { get; init; } = default!;

    public string? Description { get; init; }
}

internal sealed class ChangeGroupInfoCommandHandler : ICommandHandler<ChangeGroupInfoCommand>
{
    private readonly IGroupRepository _groupRepository;

    public ChangeGroupInfoCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(ChangeGroupInfoCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.ChangeName(command.Name);
        group.ChangeDescription(command.Description);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return Unit.Value;
    }
}