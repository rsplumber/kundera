using Mediator;
using Queries;
using Queries.Services;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Services;

public sealed class ServicesQueryHandler : IQueryHandler<ServicesQuery, PageableResponse<ServicesResponse>>
{
    private IRedisCollection<ServiceDataModel> _services;

    public ServicesQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>)provider.RedisCollection<ServiceDataModel>(false);
    }

    public async ValueTask<PageableResponse<ServicesResponse>> Handle(ServicesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _services = _services.Where(model => model.Name.Contains(query.Name));
        }

        var serviceQuery = await _services.Page(query).ToListAsync();
        var services = serviceQuery.Select(model => new ServicesResponse(model.Id, model.Name, model.Status)).ToList();
        int counts;
        if (query.Name is not null)
        {
            counts = await _services.Where(model => model.Name.Contains(query.Name)).CountAsync();
        }
        else
        {
            counts = await _services.CountAsync();
        }

        return new PageableResponse<ServicesResponse>
        {
            Data = services,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}