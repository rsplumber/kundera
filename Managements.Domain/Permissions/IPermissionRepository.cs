using Kite.Domain.Contracts;

namespace Managements.Domain.Permissions;

public interface IPermissionRepository : IRepository<Permission, PermissionId>, IUpdateService<Permission>, IDeleteService<PermissionId>
{
    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<List<Permission>> FindAllAsync(CancellationToken cancellationToken = default);
}