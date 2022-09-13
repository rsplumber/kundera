using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;

namespace RoleManagements.Domain.Tests.Permissions;

public class PermissionRepository : IPermissionRepository
{
    private static readonly List<Permission> permissions = new();

    public async Task CreateAsync(Permission entity, CancellationToken cancellationToken = new CancellationToken())
    {
        permissions.Add(entity);
    }

    public async Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = new CancellationToken())
    {
        return permissions.FirstOrDefault(service => service.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return permissions.Exists(service => service.Id == id);
    }
}