﻿using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Permissions;

namespace Managements.Application.Permissions;

public sealed record CreatePermissionCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand>
{
    private readonly IPermissionFactory _permissionFactory;
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IPermissionFactory permissionFactory)
    {
        _permissionRepository = permissionRepository;
        _permissionFactory = permissionFactory;
    }

    public async Task HandleAsync(CreatePermissionCommand message, CancellationToken cancellationToken = default)
    {
        var (name, meta) = message;
        var permission = await _permissionFactory.CreateAsync(name);
        if (meta is not null)
        {
            foreach (var (key, value) in meta)
            {
                permission.Meta.Add(key, value);
            }
        }

        await _permissionRepository.AddAsync(permission, cancellationToken);
    }
}