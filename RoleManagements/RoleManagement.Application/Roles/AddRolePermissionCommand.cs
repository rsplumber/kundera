using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class AddRolePermissionCommandHandler : CommandHandler<AddRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;

    public AddRolePermissionCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(AddRolePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, permissions) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permission in permissions)
        {
            role.AddPermission(permission);
        }
    }
}