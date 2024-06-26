﻿using Core.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Data.Permissions;

internal class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Permissions.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Permission?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Permissions
            .FirstOrDefaultAsync(permission => permission.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Permissions.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var currentPermission = await _dbContext.Permissions
            .FirstOrDefaultAsync(permission => permission.Id == id, cancellationToken);
        if (currentPermission is null) return;
        _dbContext.Permissions.Remove(currentPermission);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}