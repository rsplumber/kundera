using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Types;

namespace RoleManagements.Domain.Tests.Roles;

public class RoleRepository : IRoleRepository
{
    private static readonly List<Role> Roles = new();

    public async Task CreateAsync(Role entity, CancellationToken cancellationToken = new())
    {
        Roles.Add(entity);
    }

    public async Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = new())
    {
        return Roles.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return Roles.Exists(service => service.Id == id);
    }
}