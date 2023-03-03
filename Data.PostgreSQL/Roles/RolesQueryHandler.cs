using Application.Roles;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Roles;

public sealed class RolesQueryHandler : IQueryHandler<RolesQuery, List<RolesResponse>>
{
    private readonly AppDbContext _dbContext;

    public RolesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<RolesResponse>> Handle(RolesQuery query, CancellationToken cancellationToken)
    {
        
        var dbQuery = _dbContext.Roles.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        return await dbQuery
            .AsNoTracking()
            .Select(model => new RolesResponse(model.Id, model.Name))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}