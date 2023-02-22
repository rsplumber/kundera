namespace Core.Domains.Roles;

public interface IRoleRepository
{
    Task AddAsync(Role entity, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task UpdateAsync(Role entity, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}