using AutoMapper;
using Managements.Domain;
using Managements.Domain.Roles;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Roles;

internal class RoleRepository : IRoleRepository
{
    private readonly RedisCollection<RoleDataModel> _roles;
    private readonly IMapper _mapper;

    public RoleRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.InsertAsync(role);
    }

    public async Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        var roleDataModel = await _roles.FindByIdAsync(id.ToString());

        return _mapper.Map<Role>(roleDataModel);
    }

    public async Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default)
    {
        return await _roles.AnyAsync(model => model.Name == name.Value);
    }

    public async Task<IEnumerable<Role>> FindAsync(IEnumerable<RoleId> roleIds, CancellationToken cancellationToken = default)
    {
        var rawRoleIds = roleIds.Select(id => id.ToString());
        var dataModels = await _roles.FindByIdsAsync(rawRoleIds);

        return dataModels.Values.Select(model => _mapper.Map<Role>(model));
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.UpdateAsync(role);
    }

    public Task DeleteAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}