using Core.Domains.Groups;
using Mediator;

namespace Commands.Groups;

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