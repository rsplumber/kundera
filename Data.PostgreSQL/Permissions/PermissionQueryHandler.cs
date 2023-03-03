﻿using Application.Permissions;
using Core.Domains.Permissions.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Permissions;

public sealed class PermissionQueryHandler : IQueryHandler<PermissionQuery, PermissionResponse>
{
    private readonly AppDbContext _dbContext;

    public PermissionQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PermissionResponse> Handle(PermissionQuery query, CancellationToken cancellationToken)
    {
        var permission = await _dbContext.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.PermissionId, cancellationToken: cancellationToken);
        if (permission is null)
        {
            throw new PermissionNotFoundException();
        }
        return new PermissionResponse
        {
            Id = permission.Id,
            Name = permission.Name,
            Meta = permission.Meta,
        };
    }
}