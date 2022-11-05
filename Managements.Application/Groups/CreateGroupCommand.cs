using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Roles;

namespace Managements.Application.Groups;

public sealed record CreateGroupCommand(Name Name, RoleId Role, GroupId? Parent = null) : Command;

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand>
{
    private readonly IGroupFactory _groupFactory;

    public CreateGroupCommandHandler(IGroupFactory groupFactory)
    {
        _groupFactory = groupFactory;
    }

    public async Task HandleAsync(CreateGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId, parent) = message;
        await _groupFactory.CreateAsync(name, roleId, parent);
    }
}