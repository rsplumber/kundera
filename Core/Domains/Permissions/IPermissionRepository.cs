namespace Core.Domains.Permissions;

public interface IPermissionRepository
{
    Task AddAsync(Permission entity, CancellationToken cancellationToken = default);

    Task<Permission?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Permission>> FindAsync(CancellationToken cancellationToken = default);

    Task<Permission?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}