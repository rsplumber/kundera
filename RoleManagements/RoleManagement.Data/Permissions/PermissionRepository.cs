using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagement.Data.Permissions;

internal class PermissionRepository : IPermissionRepository 
{
    public async Task AddAsync(Permission entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Permission entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}