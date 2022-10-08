using Application.Services;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Services;

internal sealed class ServicesQueryHandler : QueryHandler<ServicesQuery, IEnumerable<ServicesResponse>>
{
    private IRedisCollection<ServiceDataModel> _services;

    public ServicesQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public override async Task<IEnumerable<ServicesResponse>> HandleAsync(ServicesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _services = _services.Where(model => model.Id.Contains(message.Name));
        }

        var serviceDataModels = await _services.ToListAsync();

        return serviceDataModels.Select(model => new ServicesResponse(model.Id, model.Status));
    }
}