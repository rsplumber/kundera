using Data.Abstractions;
using Data.Abstractions.Roles;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Roles;

public sealed class RolesQueryHandler : IQueryHandler<RolesQuery, PageableResponse<RolesResponse>>
{
    private readonly AppDbContext _dbContext;

    public RolesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<RolesResponse>> Handle(RolesQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Roles.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var roles = await dbQuery
            .AsNoTracking()
            .Page(query)
            .Select(model => new RolesResponse(model.Id, model.Name))
            .ToListAsync(cancellationToken: cancellationToken);

        var countsQuery = _dbContext.Roles.AsQueryable();
        if (query.Name is not null)
        {
            countsQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var counts = await countsQuery.CountAsync(cancellationToken);
        return new PageableResponse<RolesResponse>
        {
            Data = roles,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}