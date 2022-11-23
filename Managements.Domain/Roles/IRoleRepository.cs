using Managements.Domain.Roles.Types;

namespace Managements.Domain.Roles;

public interface IRoleRepository
{
    Task AddAsync(Role entity, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(RoleId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<Role?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> FindAsync(IEnumerable<RoleId> roleIds, CancellationToken cancellationToken = default);

    Task UpdateAsync(Role entity, CancellationToken cancellationToken = default);

    public Task DeleteAsync(RoleId id, CancellationToken cancellationToken = default);
}