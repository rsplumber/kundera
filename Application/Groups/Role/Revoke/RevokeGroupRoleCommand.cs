using Core.Groups;
using Core.Groups.Exception;
using Core.Roles;
using Core.Roles.Exceptions;
using Mediator;

namespace Application.Groups.Role.Revoke;

public sealed record RevokeGroupRoleCommand : ICommand
{
    public Guid GroupId { get; init; } = default!;

    public Guid[] RolesIds { get; init; } = default!;
}

internal sealed class RevokeGroupRoleCommandHandler : ICommandHandler<RevokeGroupRoleCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IRoleRepository _roleRepository;


    public RevokeGroupRoleCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
    {
        _groupRepository = groupRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(RevokeGroupRoleCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(command.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        foreach (var roleId in command.RolesIds)
        {
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }
            group.Revoke(role);
        }

        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}