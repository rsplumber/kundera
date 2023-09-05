using Core.Services.Exceptions;
using Data.Abstractions.Services;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Services;

public sealed class ServiceQueryHandler : IQueryHandler<ServiceQuery, ServiceResponse>
{
    private readonly IRedisCollection<ServiceDataModel> _services;

    public ServiceQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>)provider.RedisCollection<ServiceDataModel>(false);
    }

    public async ValueTask<ServiceResponse> Handle(ServiceQuery query, CancellationToken cancellationToken)
    {
        var service = await _services.FindByIdAsync(query.ServiceId.ToString());
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Secret = service.Secret,
            Status = service.Status
        };
    }
}