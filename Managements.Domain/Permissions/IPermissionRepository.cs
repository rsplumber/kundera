using Kite.Domain.Contracts;

namespace Managements.Domain.Permissions;

public interface IPermissionRepository : IRepository<Permission, PermissionId>, IUpdateService<Permission>, IDeleteService<PermissionId>
{
    ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default);

    ValueTask<List<Permission>> FindAllAsync();
}