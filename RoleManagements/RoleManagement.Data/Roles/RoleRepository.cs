using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Types;

namespace RoleManagement.Data.Roles;

internal class RoleRepository : IRoleRepository
{
    public async Task AddAsync(Role entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Role entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}