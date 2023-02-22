using AutoMapper;
using Core.Domains.Roles;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal class RoleRepository : IRoleRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<RoleDataModel> _roles;
    private readonly IMapper _mapper;

    public RoleRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _roles = (RedisCollection<RoleDataModel>)provider.RedisCollection<RoleDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<RoleDataModel>(entity);
        await _roles.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Role?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _roles.FindByIdAsync(id.ToString());
        return _mapper.Map<Role>(dataModel);
    }

    public async Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _roles.FirstOrDefaultAsync(model => model.Name == name);
        return _mapper.Map<Role>(dataModel);
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.InsertAsync(role);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}