using Kite.Domain.Contracts;

namespace Domain.Permissions;

public interface IPermissionRepository : IRepository<Permission,PermissionId>, IUpdateService<Permission>, IDeleteService<PermissionId>
{
    ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default);

    Task<List<Permission>> FindAllAsync();
}