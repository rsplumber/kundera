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
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateGroupCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async Task HandleAsync(CreateGroupCommand message, CancellationToken cancellationToken = default)
    {
        var (name, roleId) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        var group = await Group.FromAsync(name, role.Id, _groupRepository);
        await _groupRepository.AddAsync(group, cancellationToken);
    }
}