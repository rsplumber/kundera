using RoleManagements.Domain.UserRoles;

namespace RoleManagements.Domain.Tests.UserRoles;

public class UserRolesRepository : IUserRoleRepository
{
    private readonly List<UserRole> _userRoles;

    public UserRolesRepository()
    {
        _userRoles = new List<UserRole>();
    }

    public async Task CreateAsync(UserRole entity, CancellationToken cancellationToken = new())
    {
        _userRoles.Add(entity);
    }

    public async Task<UserRole?> FindAsync(UserId id, CancellationToken cancellationToken = new())
    {
        return _userRoles.FirstOrDefault(userRole => userRole.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return _userRoles.Exists(userRole => userRole.Id == id);
    }
}