using Application.Services;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Services;

public sealed class ServicesQueryHandler : IQueryHandler<ServicesQuery, List<ServicesResponse>>
{
    private readonly AppDbContext _dbContext;

    public ServicesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<ServicesResponse>> Handle(ServicesQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Services.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        return await dbQuery
            .AsNoTracking()
            .Select(model => new ServicesResponse(model.Id, model.Name, model.Status.ToString()))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}