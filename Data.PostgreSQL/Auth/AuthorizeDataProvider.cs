using Core.Domains.Auth;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth;

internal sealed class AuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly AppDbContext _dbContext;

    private const string GroupsChildrenRawQuery =
        @"
WITH RECURSIVE group_tree(id, parent_id, name, status) AS (
    SELECT g.id, g.parent_id, g.name, g.status
    FROM groups g WHERE g.id = {0}
    UNION ALL
    SELECT g.id, g.parent_id, g.name, g.status
    FROM groups g JOIN group_tree gt ON g.parent_id = gt.id
)
SELECT DISTINCT roles.*
FROM group_tree
JOIN groups_roles ON groups_roles.group_id = group_tree.id
JOIN roles ON roles.id = groups_roles.role_id
JOIN roles_permission ON roles_permission.role_id = roles.id
JOIN permissions ON permission.id = roles_permission.permission_id;
";

    public AuthorizeDataProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        return _dbContext.Sessions
            .AsNoTracking()
            .Where(session => session.Id == sessionToken)
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
                        Status = ug.Status,
                        Roles = ug.Roles.Select(role => new Role
                        {
                            Id = role.Id,
                            Name = role.Name,
                            Meta = role.Meta,
                            Permissions = role.Permissions
                        }).ToList()
                    }).ToList()
                },
                Activity = new AuthActivity
                {
                    Id = session.Activity.Id,
                    Agent = session.Activity.Agent,
                    CreatedDateUtc = session.Activity.CreatedDateUtc
                },
                Scope = new Scope
                {
                    Id = session.Scope.Id,
                    Status = session.Scope.Status,
                    Roles = session.Scope.Roles.ToList(),
                    Services = session.Scope.Services.ToList()
                },
                ExpirationDateUtc = session.ExpirationDateUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var allRoles = new List<Role>();
        allRoles.AddRange(user.Roles);
        allRoles.AddRange(user.Groups.SelectMany(g => g.Roles));

        foreach (var group in user.Groups)
        {
            var childGroups = await GetAllChildrenRolesAsync(group, cancellationToken);
            allRoles.AddRange(childGroups);
        }

        return allRoles;
    }

    private Task<List<Role>> GetAllChildrenRolesAsync(Group group, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles.FromSqlRaw(GroupsChildrenRawQuery, group.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        return _dbContext.Services
            .AsNoTracking()
            .Where(service => service.Secret == serviceSecret)
            .Select(service => new Service
            {
                Id = service.Id,
                Name = service.Name,
                Status = service.Status
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}