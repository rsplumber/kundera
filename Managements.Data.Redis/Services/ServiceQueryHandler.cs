using Managements.Application.Services;
using Managements.Domain.Services.Exceptions;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Services;

internal sealed class ServiceQueryHandler : IQueryHandler<ServiceQuery, ServiceResponse>
{
    private readonly IRedisCollection<ServiceDataModel> _services;

    public ServiceQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public async ValueTask<ServiceResponse> Handle(ServiceQuery query, CancellationToken cancellationToken)
    {
        var service = await _services.FindByIdAsync(query.Service.ToString());
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