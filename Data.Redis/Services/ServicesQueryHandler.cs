using Application.Services;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Services;

internal sealed class ServicesIQueryHandler : IQueryHandler<ServicesQuery, IEnumerable<ServicesResponse>>
{
    private IRedisCollection<ServiceDataModel> _services;

    public ServicesIQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public async ValueTask<IEnumerable<ServicesResponse>> HandleAsync(ServicesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _services = _services.Where(model => model.Id.Contains(message.Name));
        }

        var serviceDataModels = await _services.ToListAsync();

        return serviceDataModels.Select(model => new ServicesResponse(model.Id, model.Status));
    }
}