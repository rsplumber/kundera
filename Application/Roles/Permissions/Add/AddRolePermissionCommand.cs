﻿using Core.Permissions;
using Core.Permissions.Exceptions;
using Core.Roles;
using Core.Roles.Exceptions;
using Mediator;

namespace Application.Roles.Permissions.Add;

public sealed record AddRolePermissionCommand : ICommand
{
    public Guid RoleId { get; init; } = default!;

    public Guid[] PermissionsIds { get; init; } = default!;
}

internal sealed class AddRolePermissionCommandHandler : ICommandHandler<AddRolePermissionCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AddRolePermissionCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async ValueTask<Unit> Handle(AddRolePermissionCommand command, CancellationToken cancellationToken)
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

            role.Add(permission);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);

        return Unit.Value;
    }
}