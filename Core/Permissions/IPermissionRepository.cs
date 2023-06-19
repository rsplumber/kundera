namespace Core.Permissions;

public interface IPermissionRepository
{
    Task AddAsync(Permission entity, CancellationToken cancellationToken = default);

    Task<Permission?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}