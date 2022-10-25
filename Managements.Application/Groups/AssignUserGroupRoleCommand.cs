using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Groups;

public sealed record AssignGroupRoleCommand(GroupId Group, params RoleId[] Roles) : Command;

internal sealed class AssignGroupRoleCommandHandler : ICommandHandler<AssignGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;

    public AssignGroupRoleCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async Task HandleAsync(AssignGroupRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, roleIds) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var roleId in roleIds)
        {
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            group.AssignRole(role.Id);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}