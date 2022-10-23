﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;

namespace Managements.Application.Roles;

public sealed record DeleteRoleCommand(RoleId Id) : Command;

internal sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task HandleAsync(DeleteRoleCommand message, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.FindAsync(message.Id, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteAsync(role.Id, cancellationToken);
    }
}