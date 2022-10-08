using AutoMapper;
using Domain.Permissions;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Permissions;

internal class PermissionRepository : IPermissionRepository
{
    private readonly RedisCollection<PermissionDataModel> _permissions;
    private readonly IMapper _mapper;

    public PermissionRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _permissions = (RedisCollection<PermissionDataModel>) provider.RedisCollection<PermissionDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var permission = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.InsertAsync(permission);
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        var permissionDataModel = await _permissions.FindByIdAsync(id.Value);
        return _mapper.Map<Permission>(permissionDataModel);
    }

    public async ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return await _permissions.AnyAsync(model => model.Id == id.Value);
    }

    public async Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
    {
        var permission = _mapper.Map<PermissionDataModel>(entity);
        await _permissions.UpdateAsync(permission);
    }
}