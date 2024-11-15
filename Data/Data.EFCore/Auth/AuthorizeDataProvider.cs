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
    private const string GroupsChildrenAllRolesRawQuery =
        """
        WITH RECURSIVE group_tree(id, parent_id) AS (SELECT g.id, g.parent_id FROM groups g WHERE g.id = {0}
        UNION ALL SELECT g.id, g.parent_id FROM groups g
        JOIN group_tree gt ON g.parent_id = gt.id)
        SELECT DISTINCT r.* FROM group_tree as g
        JOIN groups_roles AS gr
        ON g.id = gr.group_id
        JOIN roles AS r ON gr.role_id = r.id
        GROUP BY r.id
        """;


    private static readonly Func<AppDbContext, string, Task<Session?>>? CurrentSessionCompileAsyncQuery = EF.CompileAsyncQuery((AppDbContext dbContext, string st) =>
        dbContext.Sessions
            .Include(session => session.User)
            .ThenInclude(user => user.Groups)
            .Include(session => session.Scope)
            .ThenInclude(scope => scope.Roles)
            .Include(session => session.User)
            .ThenInclude(user => user.Roles)
            .ThenInclude(role => role.Permissions)
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
                TokenExpirationDateUtc = session.TokenExpirationDateUtc,
                RefreshTokenExpirationDateUtc = session.TokenExpirationDateUtc
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

    private readonly AppDbContext _dbContext;

    public AuthorizeDataProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Session?> FindSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        return CurrentSessionCompileAsyncQuery!(_dbContext, sessionToken);
    }

    public async Task<List<Role>> FindUserRolesAsync(User user, CancellationToken cancellationToken = default)
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

    public Task<Service?> FindServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        return ServiceCompiledQuery!(_dbContext, serviceSecret);
    }

    private Task<List<Role>> GetAllChildrenRolesAsync(Group group, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles
            .FromSqlRaw(GroupsChildrenAllRolesRawQuery, group.Id)
            .Include(role => role.Permissions)
            .Select(role => new Role
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions.Select(permission => new Permission
                {
                    Id = permission.Id,
                    Name = permission.Name
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}