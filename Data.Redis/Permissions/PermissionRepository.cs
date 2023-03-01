using AutoMapper;
using Core.Domains.Permissions;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Permissions;

internal class PermissionRepository : IPermissionRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<PermissionDataModel> _permissions;
    private readonly IMapper _mapper;

    public PermissionRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _permissions = (RedisCollection<PermissionDataModel>)provider.RedisCollection<PermissionDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Permission?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _permissions.FindByIdAsync(id.ToString());
        return _mapper.Map<Permission>(dataModel);
    }

    public async Task<IEnumerable<Permission>> FindAsync(CancellationToken cancellationToken = default)
    {
        var dataModels = await _permissions.ToListAsync();
        return _mapper.Map<List<Permission>>(dataModels);
    }

    public async Task<Permission?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _permissions.FirstOrDefaultAsync(model => model.Name == name);
        return _mapper.Map<Permission>(dataModel);
    }

    public async Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var permission = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(permission);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}