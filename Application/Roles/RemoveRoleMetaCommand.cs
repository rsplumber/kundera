﻿using Domain.Roles;
using Domain.Roles.Exceptions;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Roles;

public sealed record RemoveRoleMetaCommand(RoleId Role, params string[] MetaKeys) : Command;

internal sealed class RemoveRoleMetaCommandHandler : ICommandHandler<RemoveRoleMetaCommand>
{
    private readonly IRoleRepository _roleRepository;

    public RemoveRoleMetaCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(RemoveRoleMetaCommand message, CancellationToken cancellationToken = default)
    {
        var (roleId, metaKeys) = message;
        var role = await _roleRepository.FindAsync(roleId, cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        foreach (var metaKey in metaKeys)
        {
            role.RemoveMeta(metaKey);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
    }
}