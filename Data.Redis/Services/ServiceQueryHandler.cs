using Application.Services;
using Core.Domains.Services.Exceptions;
using Managements.Data.ConnectionProviders;
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

        return new ServiceResponse(service.Id, service.Name, service.Secret, service.Status);
    }
}