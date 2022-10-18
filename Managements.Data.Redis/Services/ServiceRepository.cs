﻿using AutoMapper;
using Managements.Domain;
using Managements.Domain.Services;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Services;

internal class ServiceRepository : IServiceRepository
{
    private readonly RedisCollection<ServiceDataModel> _services;
    private readonly IMapper _mapper;


    public ServiceRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var service = _mapper.Map<ServiceDataModel>(entity);
        await _services.InsertAsync(service);
    }

    public async Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        var serviceDataModel = await _services.FindByIdAsync(id.ToString());

        return _mapper.Map<Service>(serviceDataModel);
    }

    public async Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default)
    {
        return await _services.AnyAsync(model => model.Name == name);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var service = _mapper.Map<ServiceDataModel>(entity);
        await _services.UpdateAsync(service);
    }

    public Task DeleteAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}