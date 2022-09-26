using RoleManagements.Domain.Permissions.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Permissions;

public interface IPermissionRepository : IRepository<PermissionId, Permission>, IUpdateService<Permission>
{
    ValueTask<bool> ExistsAsync(PermissionId id, CancellationToken cancellationToken = default);
}