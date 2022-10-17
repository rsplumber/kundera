using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record AssignUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

internal sealed class AssignUserGroupRoleCommandHandler : ICommandHandler<AssignUserGroupRoleCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IRoleRepository _roleRepository;

    public AssignUserGroupRoleCommandHandler(IUserGroupRepository userGroupRepository, IRoleRepository roleRepository)
    {
        _userGroupRepository = userGroupRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(AssignUserGroupRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, roleIds) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
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

        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}