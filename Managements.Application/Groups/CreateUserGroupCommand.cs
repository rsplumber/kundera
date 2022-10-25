using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Groups;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Groups;

public sealed record CreateGroupCommand(Name Name, RoleId Role) : Command;

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand>
{
    private readonly IGroupFactory _groupFactory;
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateGroupCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository, IGroupFactory groupFactory)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
        _groupFactory = groupFactory;
    }

    public async Task HandleAsync(CreateGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var group = await _groupFactory.CreateAsync(name, role.Id);
        await _groupRepository.AddAsync(group, cancellationToken);
    }
}