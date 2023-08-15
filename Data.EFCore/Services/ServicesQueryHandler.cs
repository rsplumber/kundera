using Data.Abstractions;
using Data.Abstractions.Services;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public sealed class ServicesQueryHandler : IQueryHandler<ServicesQuery, PageableResponse<ServicesResponse>>
{
    private readonly AppDbContext _dbContext;

    public ServicesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<ServicesResponse>> Handle(ServicesQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Services.AsQueryable();
        if (query.Name is not null)
        {
            dbQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var services = await dbQuery
            .AsNoTracking()
            .Page(query)
            .Select(model => new ServicesResponse(model.Id, model.Name, model.Status.ToString()))
            .ToListAsync(cancellationToken: cancellationToken);

        var countsQuery = _dbContext.Services.AsQueryable();
        if (query.Name is not null)
        {
            countsQuery = dbQuery.Where(model => model.Name.Contains(query.Name));
        }

        var counts = await countsQuery.CountAsync(cancellationToken);
        return new PageableResponse<ServicesResponse>
        {
            Data = services,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}