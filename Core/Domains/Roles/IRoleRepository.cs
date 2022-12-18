using Core.Domains.Roles.Types;

namespace Core.Domains.Roles;

public interface IRoleRepository
{
    Task AddAsync(Role entity, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task UpdateAsync(Role entity, CancellationToken cancellationToken = default);

    public Task DeleteAsync(RoleId id, CancellationToken cancellationToken = default);
}