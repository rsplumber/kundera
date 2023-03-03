using Application.Roles;
using Core.Domains.Roles.Exceptions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Roles;

public sealed class RoleQueryHandler : IQueryHandler<RoleQuery, RoleResponse>
{
    private readonly AppDbContext _dbContext;

    public RoleQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<RoleResponse> Handle(RoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles
            .AsNoTracking()
            .Include(role => role.Permissions)
            .FirstOrDefaultAsync(role => role.Id == query.RoleId , cancellationToken);
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Meta = role.Meta,
            Permissions = role.Permissions.Select(permission => permission.Id)
        };
    }
}