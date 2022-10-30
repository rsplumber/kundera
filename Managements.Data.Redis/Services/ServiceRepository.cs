using AutoMapper;
using Kite.Events;
using Managements.Domain;
using Managements.Domain.Services;
using Managements.Domain.Services.Types;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Services;

internal class ServiceRepository : IServiceRepository
{
    private readonly IEventBus _eventBus;
    private readonly RedisCollection<ServiceDataModel> _services;
    private readonly IMapper _mapper;


    public ServiceRepository(RedisConnectionProvider provider, IMapper mapper, IEventBus eventBus)
    {
        _services = (RedisCollection<ServiceDataModel>) provider.RedisCollection<ServiceDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var service = _mapper.Map<ServiceDataModel>(entity);
        await _services.InsertAsync(service);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Service?> FindAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        var serviceDataModel = await _services.FindByIdAsync(id.ToString());

        return _mapper.Map<Service>(serviceDataModel);
    }

    public async Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default)
    {
        return await _services.AnyAsync(model => model.Name == name.Value);
    }

    public async Task<Service?> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _services.FirstOrDefaultAsync(model => model.Name == name.Value);

        return _mapper.Map<Service>(dataModel);
    }

    public async Task<Service?> FindAsync(ServiceSecret secret, CancellationToken cancellationToken = default)
    {
        var dataModel = await _services.Where(model => model.Secret == secret.Value).FirstOrDefaultAsync();

        return _mapper.Map<Service>(dataModel);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var service = _mapper.Map<ServiceDataModel>(entity);
        await _services.UpdateAsync(service);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}