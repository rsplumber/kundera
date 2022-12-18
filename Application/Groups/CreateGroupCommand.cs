using Core.Domains.Groups;
using Core.Domains.Groups.Types;
using Core.Domains.Roles.Types;
using Mediator;

namespace Application.Groups;

public sealed record CreateGroupCommand : ICommand<Group>
{
    public string Name { get; init; } = default!;

    public Guid Role { get; init; } = default!;

    public Guid? Parent { get; init; }
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
        var group = await _groupFactory.CreateAsync(command.Name,
            RoleId.From(command.Role),
            command.Parent is not null ? GroupId.From(command.Parent.Value) : null);

        return group;
    }
}