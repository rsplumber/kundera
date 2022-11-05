using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Roles;

namespace Managements.Application.Groups;

public sealed record RevokeGroupRoleCommand(GroupId Group, params RoleId[] Roles) : Command;

internal sealed class RevokeGroupRoleICommandHandler : ICommandHandler<RevokeGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;

    public RevokeGroupRoleICommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task HandleAsync(RevokeGroupRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (groupId, roleIds) = message;
        var group = await _groupRepository.FindAsync(groupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var role in roleIds)
        {
            group.RevokeRole(role);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);
    }
}