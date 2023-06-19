using Core.Permissions;
using Core.Permissions.Exceptions;
using Core.Roles;
using Core.Roles.Exceptions;
using Mediator;

namespace Application.Roles.Permissions.Delete;

public sealed record DeleteRolePermissionCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public Guid[] PermissionsIds { get; init; } = default!;
}

internal sealed class DeleteRolePermissionCommandHandler : ICommandHandler<DeleteRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public DeleteRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(DeleteRolePermissionCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FindAsync(command.RoleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var permissionId in command.PermissionsIds)
        {
            var permission = await _permissionRepository.FindAsync(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PermissionNotFoundException();
            }
            role.Remove(permission);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}