using RoleManagements.Domain.Roles.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Roles;

public interface IRoleRepository : IRepository<RoleId, Role>, IUpdateService<Role>, IDeleteService<RoleId>
{
    ValueTask<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default);
}