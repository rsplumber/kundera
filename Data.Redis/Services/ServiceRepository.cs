﻿using AutoMapper;
using Domain.Services;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Services;

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
        var serviceDataModel = await _services.FindByIdAsync(id.Value);

        return _mapper.Map<Service>(serviceDataModel);
    }

    public async ValueTask<bool> ExistsAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        return await _services.AnyAsync(model => model.Id == id.Value);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var service = _mapper.Map<ServiceDataModel>(entity);
        await _services.UpdateAsync(service);
    }

    public Task DeleteAsync(ServiceId id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}