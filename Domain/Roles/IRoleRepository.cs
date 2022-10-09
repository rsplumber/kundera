using Tes.Domain.Contracts;

namespace Domain.Roles;

public interface IRoleRepository : IRepository<RoleId, Role>, IUpdateService<Role>, IDeleteService<RoleId>
{
    ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Role>> FindAsync(RoleId[] roleIds, CancellationToken cancellationToken = default);
}