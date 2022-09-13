using Tes.Domain.Contracts;

namespace RoleManagements.Domain.UserRoles;

public interface IUserRoleRepository : IRepository<UserId, UserRole>
{
    ValueTask<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default);
}