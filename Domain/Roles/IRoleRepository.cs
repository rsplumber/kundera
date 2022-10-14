using Kite.Domain.Contracts;

namespace Domain.Roles;

public interface IRoleRepository : IRepository<Role, RoleId>, IUpdateService<Role>, IDeleteService<RoleId>
{
    ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Role>> FindAsync(RoleId[] roleIds, CancellationToken cancellationToken = default);
}