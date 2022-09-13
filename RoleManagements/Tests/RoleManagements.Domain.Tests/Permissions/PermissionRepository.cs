﻿using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagements.Domain.Tests.Permissions;

public class PermissionRepository : IPermissionRepository
{
    private readonly List<Permission> _permissions;

    public PermissionRepository()
    {
        _permissions = new List<Permission>();
    }

    public async Task CreateAsync(Permission entity, CancellationToken cancellationToken = new CancellationToken())
    {
        _permissions.Add(entity);
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _permissions.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return _permissions.Exists(service => service.Id == id);
    }
}