using Application.Services;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Services;

public sealed class ServicesQueryHandler : IQueryHandler<ServicesQuery, List<ServicesResponse>>
{
    private IRedisCollection<ServiceDataModel> _services;

    public ServicesQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>)provider.RedisCollection<ServiceDataModel>(false);
    }

    public async ValueTask<List<ServicesResponse>> Handle(ServicesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _services = _services.Where(model => model.Name.Contains(query.Name));
        }

        var serviceDataModels = await _services.ToListAsync();

        return serviceDataModels.Select(model => new ServicesResponse(model.Id, model.Name, model.Status)).ToList();
    }
}