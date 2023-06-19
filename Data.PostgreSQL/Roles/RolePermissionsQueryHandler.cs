using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Roles;

namespace Data.Roles;

public sealed class RolePermissionsQueryHandler : IQueryHandler<RolePermissionsQuery, List<RolePermissionsResponse>>
{
    private readonly AppDbContext _dbContext;

    public RolePermissionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<RolePermissionsResponse>> Handle(RolePermissionsQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .Where(role => role.Id == query.RoleId)
            .Include(role => role.Permissions)
            .SelectMany(role => role.Permissions.Select(permission => new RolePermissionsResponse(permission.Id, permission.Name)))
            .ToListAsync( cancellationToken: cancellationToken);
    }
}