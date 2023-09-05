using Data.Abstractions;
using Data.Abstractions.Scopes;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Scopes;

public sealed class ScopesQueryHandler : IQueryHandler<ScopesQuery, PageableResponse<ScopesResponse>>
{
    private readonly AppDbContext _dbContext;

    public ScopesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<ScopesResponse>> Handle(ScopesQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Scopes.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var scopes = await dbQuery
            .AsNoTracking()
            .Page(query)
            .Select(model => new ScopesResponse(model.Id, model.Name, model.Status.ToString()))
            .ToListAsync(cancellationToken: cancellationToken);

        var countsQuery = _dbContext.Scopes.AsQueryable();
        if (query.Name is not null)
        {
            countsQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var counts = await countsQuery.CountAsync(cancellationToken);
        return new PageableResponse<ScopesResponse>
        {
            Data = scopes,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}