using Core.Auth.Authorizations;
using Core.Auth.Sessions;
using Core.Groups;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth;

internal sealed class AuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly AppDbContext _dbContext;

    private const string GroupsChildrenAllRolesRawQuery =
        @"
WITH RECURSIVE group_tree(id, parent_id) AS (SELECT g.id, g.parent_id FROM groups g WHERE g.id = {0} UNION ALL SELECT g.id, g.parent_id FROM groups g JOIN group_tree gt ON g.parent_id = gt.id) SELECT DISTINCT r.* FROM group_tree as g JOIN groups_roles AS gr ON g.id = gr.group_id JOIN roles AS r ON gr.role_id = r.id GROUP BY r.id
";

    private const string RolePermissionRawQuery = @"SELECT DISTINCT p.* FROM permissions p LEFT JOIN roles_permission rp ON p.Id = rp.permission_id WHERE rp.role_id = ANY ({0})";

    private static readonly Func<AppDbContext, string, Task<Session?>>? CurrentSessionCompileAsyncQuery = EF.CompileAsyncQuery((AppDbContext dbContext, string st) =>
        dbContext.Sessions
            .Include(session => session.User)
            .ThenInclude(user => user.Groups)
            .Include(session => session.Scope)
            .ThenInclude(scope => scope.Roles)
            .Where(session => session.Id == st)
            .Select(session => new Session
            {
                Id = session.Id,
                User = new User
                {
                    Id = session.User.Id,
                    Status = session.User.Status,
                    Roles = session.User.Roles,
                    Groups = session.User.Groups.Select(ug => new Group
                    {
                        Id = ug.Id,
                        Status = ug.Status
                    }).ToList()
                },
                Scope = new Scope
                {
                    Id = session.Scope.Id,
                    Status = session.Scope.Status,
                    Roles = session.Scope.Roles,
                    Services = session.Scope.Services
                },
                ExpirationDateUtc = session.ExpirationDateUtc
            })
            .FirstOrDefault());

    private static readonly Func<AppDbContext, string, Task<Service?>>? ServiceCompiledQuery = EF.CompileAsyncQuery((AppDbContext dbContext, string secret) =>
        dbContext.Services
            .Where(service => service.Secret == secret)
            .Select(service => new Service
            {
                Id = service.Id,
                Name = service.Name,
                Status = service.Status
            })
            .FirstOrDefault());

    public AuthorizeDataProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        return CurrentSessionCompileAsyncQuery!(_dbContext, sessionToken);
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var allRoles = new List<Role>();
        allRoles.AddRange(user.Roles);
        foreach (var group in user.Groups)
        {
            var childGroupsRoles = await GetAllChildrenRolesAsync(group, cancellationToken);
            allRoles.AddRange(childGroupsRoles);
        }

        return allRoles;
    }

    public Task<Permission[]> RolePermissionsAsync(List<Role> roles, CancellationToken cancellationToken = default)
    {
        var roleIds = roles.Select(role => role.Id).ToArray();
        return _dbContext.Permissions.FromSqlRaw(RolePermissionRawQuery, roleIds)
            .ToArrayAsync(cancellationToken);
    }

    public Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        return ServiceCompiledQuery!(_dbContext, serviceSecret);
    }

    private Task<List<Role>> GetAllChildrenRolesAsync(Group group, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles
            .FromSqlRaw(GroupsChildrenAllRolesRawQuery, group.Id)
            .ToListAsync(cancellationToken);
    }
}