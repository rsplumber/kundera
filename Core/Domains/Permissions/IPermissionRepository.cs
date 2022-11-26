using Core.Domains.Permissions.Types;

namespace Core.Domains.Permissions;

public interface IPermissionRepository
{
    Task AddAsync(Permission entity, CancellationToken cancellationToken = default);

    Task<Permission?> FindAsync(PermissionId id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(PermissionId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> FindAsync(CancellationToken cancellationToken = default);

    Task<Permission> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> FindAsync(IEnumerable<PermissionId> permissionIds, CancellationToken cancellationToken = default);
}