using Core.Domains.Permissions;
using Core.Domains.Permissions.Exceptions;
using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;

namespace Application.Roles;

public sealed record RemoveRolePermissionCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public Guid[] PermissionsIds { get; init; } = default!;
}

internal sealed class RemoveRolePermissionCommandHandler : ICommandHandler<RemoveRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public RemoveRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(RemoveRolePermissionCommand command, CancellationToken cancellationToken)
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