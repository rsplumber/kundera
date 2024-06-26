﻿using Core.Groups;
using Mediator;

namespace Application.Groups.Create;

public sealed record CreateGroupCommand : ICommand<Group>
{
    public string Name { get; init; } = default!;

    public Guid RoleId { get; init; } = default!;

    public Guid? ParentId { get; init; }
}

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, Group>
{
    private readonly IGroupFactory _groupFactory;

    public CreateGroupCommandHandler(IGroupFactory groupFactory)
    {
        _groupFactory = groupFactory;
    }

    public async ValueTask<Group> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupFactory.CreateAsync(command.Name, command.RoleId, command.ParentId);
        return group;
    }
}