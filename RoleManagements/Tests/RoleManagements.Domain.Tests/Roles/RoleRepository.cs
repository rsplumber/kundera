using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Types;

namespace RoleManagements.Domain.Tests.Roles;

public class RoleRepository : IRoleRepository
{
    private readonly List<Role> _roles;

    public RoleRepository()
    {
        _roles = new List<Role>();
    }

    public async Task CreateAsync(Role entity, CancellationToken cancellationToken = new())
    {
        _roles.Add(entity);
    }

    public async Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = new())
    {
        return _roles.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return _roles.Exists(service => service.Id == id);
    }
}