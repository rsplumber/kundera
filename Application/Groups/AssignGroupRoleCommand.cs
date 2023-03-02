using Core.Domains.Groups;
using Core.Domains.Groups.Exception;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;

namespace Application.Groups;

public sealed record AssignGroupRoleCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class AssignGroupRoleCommandHandler : ICommandHandler<AssignGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;

    public AssignGroupRoleCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(AssignGroupRoleCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var roleId in command.Roles)
        {
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            group.Assign(role);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}