using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;
using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Types;

namespace RoleManagement.Data.Redis.Services;

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
        var scopeDataModel = await _services.FindByIdAsync(id.Value);
        return _mapper.Map<Service>(scopeDataModel);
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
}