using Domain.Permissions;
using Domain.Permissions.Exceptions;
using Domain.Roles;
using Domain.Roles.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Roles;

public sealed record AddRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class AddRolePermissionCommandHandler : CommandHandler<AddRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AddRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public override async Task HandleAsync(AddRolePermissionCommand message, CancellationToken cancellationToken = default)
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