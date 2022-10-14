using AutoMapper;
using Domain.Roles;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Roles;

internal class RoleRepository : IRoleRepository
{
    private readonly RedisCollection<RoleDataModel> _roles;
    private readonly IMapper _mapper;

    public RoleRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
        _mapper = mapper;
    }

    public async ValueTask AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.InsertAsync(role);
    }

    public async ValueTask<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        var roleDataModel = await _roles.FindByIdAsync(id.Value);

        return _mapper.Map<Role>(roleDataModel);
    }

    public async ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return await _roles.AnyAsync(model => model.Id == id.Value);
    }

    public async ValueTask<IEnumerable<Role>> FindAsync(RoleId[] roleIds, CancellationToken cancellationToken = default)
    {
        var rawRoleIds = roleIds.Select(id => id.Value);
        var dataModels = await _roles.FindByIdsAsync(rawRoleIds);

        return dataModels.Values.Select(model => _mapper.Map<Role>(model));
    }

    public async ValueTask UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.UpdateAsync(role);
    }

    public ValueTask DeleteAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}