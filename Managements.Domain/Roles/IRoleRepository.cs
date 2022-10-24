using Kite.Domain.Contracts;

namespace Managements.Domain.Roles;

public interface IRoleRepository : IRepository<Role, RoleId>, IUpdateService<Role>, IDeleteService<RoleId>
{
    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);
    
    Task<Role?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Role>> FindAsync(IEnumerable<RoleId> roleIds, CancellationToken cancellationToken = default);
}