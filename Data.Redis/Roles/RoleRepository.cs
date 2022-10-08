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
        _mapper = mapper;
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.InsertAsync(role);
    }

    public async Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        var roleDataModel = await _roles.FindByIdAsync(id.Value);
        return _mapper.Map<Role>(roleDataModel);
    }

    public async ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return await _roles.AnyAsync(model => model.Id == id.Value);
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = new CancellationToken())
    {
        var role = _mapper.Map<RoleDataModel>(entity);
        await _roles.UpdateAsync(role);
    }

    public Task DeleteAsync(RoleId id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}