using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;
using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagement.Data.Redis.Permissions;

internal class PermissionRepository : IPermissionRepository
{
    private readonly RedisCollection<PermissionDataModel> _permissions;
    private readonly IMapper _mapper;

    public PermissionRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = new CancellationToken())
    {
        var role = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(role);
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = new CancellationToken())
    {
        var roleDataModel = await _permissions.FindByIdAsync(id.Value);
        return _mapper.Map<Permission>(roleDataModel);
    }

    public async ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return await _permissions.AnyAsync(model => model.Id == id.Value);
    }

    public async Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var role = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.UpdateAsync(role);
    }
}