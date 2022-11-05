using Kite.CQRS;
using Managements.Application.Services;
using Managements.Domain.Services.Exceptions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Services;

internal sealed class ServiceQueryHandler : IQueryHandler<ServiceQuery, ServiceResponse>
{
    private readonly IRedisCollection<ServiceDataModel> _services;

    public ServiceQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public async Task<ServiceResponse> HandleAsync(ServiceQuery message, CancellationToken cancellationToken = default)
    {
        var service = await _services.FindByIdAsync(message.Service.ToString());
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        return new ServiceResponse(service.Id, service.Name, service.Secret, service.Status);
    }
}