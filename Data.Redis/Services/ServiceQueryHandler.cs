﻿using Application.Services;
using Domain.Services.Exceptions;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Services;

internal sealed class ServiceQueryHandler : QueryHandler<ServiceQuery, ServiceResponse>
{
    private readonly IRedisCollection<ServiceDataModel> _services;

    public ServiceQueryHandler(RedisConnectionProvider provider)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
    }

    public override async Task<ServiceResponse> HandleAsync(ServiceQuery message, CancellationToken cancellationToken = default)
    {
        var service = await _services.FindByIdAsync(message.Service.Value);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        return new ServiceResponse(service.Id, service.Status);
    }
}