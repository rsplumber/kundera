using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;
using Users.Domain.UserGroups.Exception;

namespace Users.Application.UserGroups;

public sealed record AssignUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

public sealed record RevokeUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

internal sealed class AssignUserGroupRoleCommandHandler : CommandHandler<AssignUserGroupRoleCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public AssignUserGroupRoleCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(AssignUserGroupRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (userGroupId, roleIds) = message;
        var group = await _userGroupRepository.FindAsync(userGroupId, cancellationToken);
        if (group is null)
        {
            throw new UserGroupNotFoundException();
        }

        foreach (var role in roleIds)
        {
            group.AssignRole(role);
        }

        await _userGroupRepository.UpdateAsync(group, cancellationToken);
    }
}

internal sealed class RevokeUserGroupRoleCommandHandler : CommandHandler<RevokeUserGroupRoleCommand>
{
    private readonly IUserGroupRepository _userGroupRepository;

    public RevokeUserGroupRoleCommandHandler(IUserGroupRepository userGroupRepository)
    {
        _userGroupRepository = userGroupRepository;
    }

    public override async Task HandleAsync(RevokeUserGroupRoleCommand message, CancellationToken cancellationToken = default)
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