using AutoMapper;
using Core.Domains.Services;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Services;

internal class ServiceRepository : IServiceRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<ServiceDataModel> _services;
    private readonly IMapper _mapper;


    public ServiceRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _services = (RedisCollection<ServiceDataModel>)provider.RedisCollection<ServiceDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<ServiceDataModel>(entity);
        await _services.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Service?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _services.FindByIdAsync(id.ToString());
        return _mapper.Map<Service>(dataModel);
    }

    public async Task<Service?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _services.FirstOrDefaultAsync(model => model.Name == name);
        return _mapper.Map<Service>(dataModel);
    }

    public async Task<Service?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default)
    {
        var dataModel = await _services.Where(model => model.Secret == secret).FirstOrDefaultAsync();
        return _mapper.Map<Service>(dataModel);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<ServiceDataModel>(entity);
        await _services.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}