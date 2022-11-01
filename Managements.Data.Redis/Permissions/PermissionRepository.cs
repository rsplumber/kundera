using AutoMapper;
using Kite.Events;
using Managements.Domain;
using Managements.Domain.Permissions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Permissions;

internal class PermissionRepository : IPermissionRepository
{
    private readonly IEventBus _eventBus;
    private readonly RedisCollection<PermissionDataModel> _permissions;
    private readonly IMapper _mapper;

    public PermissionRepository(RedisConnectionProvider provider, IMapper mapper, IEventBus eventBus)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _permissions.FindByIdAsync(id.ToString());
        return _mapper.Map<Permission>(dataModel);
    }

    public async Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default)
    {
        return await _permissions.AnyAsync(model => model.Name == name.Value);
    }

    public async Task<IEnumerable<Permission>> FindAsync(CancellationToken cancellationToken = default)
    {
        var dataModels = await _permissions.ToListAsync();
        return _mapper.Map<List<Permission>>(dataModels);
    }

    public async Task<Permission> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _permissions.FirstOrDefaultAsync(model => model.Name == name.Value);
        return _mapper.Map<Permission>(dataModel);
    }

    public async Task<IEnumerable<Permission>> FindAsync(IEnumerable<PermissionId> permissionIds, CancellationToken cancellationToken = default)
    {
        var rawPermissionIds = permissionIds.Select(id => id.ToString());
        var dataModels = await _permissions.FindByIdsAsync(rawPermissionIds);
        return dataModels.Values.Select(model => _mapper.Map<Permission>(model));
    }

    public async Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var permission = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(permission);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}