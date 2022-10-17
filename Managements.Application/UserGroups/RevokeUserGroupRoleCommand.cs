using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;

namespace Managements.Application.UserGroups;

public sealed record RevokeUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

internal sealed class RevokeUserGroupRoleICommandHandler : ICommandHandler<RevokeUserGroupRoleCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public RevokeUserGroupRoleICommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public async ValueTask HandleAsync(RevokeUserGroupRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, roleIds) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        foreach (var role in roleIds)
        {
            group.RevokeRole(role);
        }

        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}