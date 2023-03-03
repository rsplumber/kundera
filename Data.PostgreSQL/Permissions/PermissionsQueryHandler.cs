using Application.Permissions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Permissions;

public sealed class PermissionsQueryHandler : IQueryHandler<PermissionsQuery, List<PermissionsResponse>>
{
    private readonly AppDbContext _dbContext;

    public PermissionsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<PermissionsResponse>> Handle(PermissionsQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Permissions.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        return await dbQuery
            .AsNoTracking()
            .Select(model => new PermissionsResponse(model.Id, model.Name))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}