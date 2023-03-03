using Application.Scopes;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Scopes;

public sealed class ScopesQueryHandler : IQueryHandler<ScopesQuery, List<ScopesResponse>>
{
    private readonly AppDbContext _dbContext;

    public ScopesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<ScopesResponse>> Handle(ScopesQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Scopes.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        return await dbQuery
            .AsNoTracking()
            .Select(model => new ScopesResponse(model.Id, model.Name, model.Status.ToString()))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}