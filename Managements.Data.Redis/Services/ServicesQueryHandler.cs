using Kite.CQRS;
using Managements.Application.Services;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Services;

internal sealed class ServicesQueryHandler : IQueryHandler<ServicesQuery, IEnumerable<ServicesResponse>>
{
    private IRedisCollection<ServiceDataModel> _services;

    public ServicesQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public async Task<IEnumerable<ServicesResponse>> HandleAsync(ServicesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _services = _services.Where(model => model.Name.Contains(message.Name));
        }

        var serviceDataModels = await _services.ToListAsync();

        return serviceDataModels.Select(model => new ServicesResponse(model.Id, model.Name, model.Status));
    }
}