using Domain.Permissions;
using Domain.Roles;
using Domain.Roles.Exceptions;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Roles;

public sealed record RemoveRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class RemoveRolePermissionCommandHandler : ICommandHandler<RemoveRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRolePermissionCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(RemoveRolePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, permissions) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permission in permissions)
        {
            role.RemovePermission(permission);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
    }
}