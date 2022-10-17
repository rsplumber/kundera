using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Permissions;
using Managements.Domain.Permissions.Exceptions;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Roles;

public sealed record AddRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class AddRolePermissionCommandHandler : ICommandHandler<AddRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AddRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask HandleAsync(AddRolePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, permissions) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permissionId in permissions)
        {
            var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PermissionNotFoundException();
            }

            role.AddPermission(permission.Id);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
    }
}