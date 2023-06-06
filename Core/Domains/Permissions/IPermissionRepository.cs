namespace Core.Domains.Permissions;

public interface IPermissionRepository
{
    Task<Permission?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}