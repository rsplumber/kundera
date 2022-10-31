using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Roles;

namespace Managements.Application.Groups;

public sealed record CreateGroupCommand(Name Name, RoleId Role) : Command;

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand>
{
    private readonly IGroupFactory _groupFactory;
    private readonly IGroupRepository _groupRepository;

    public CreateGroupCommandHandler(IGroupRepository groupRepository, IGroupFactory groupFactory)
    {
        _groupRepository = groupRepository;
        _groupFactory = groupFactory;
    }

    public async Task HandleAsync(CreateGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var group = await _groupFactory.CreateAsync(name, roleId);
        await _groupRepository.AddAsync(group, cancellationToken);
    }
}