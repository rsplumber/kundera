using System.Net;
using Core.Domains;
using Core.Domains.Users.Types;
using Core.Services;
using Managements.Data.Auth.Sessions;
using Managements.Data.Groups;
using Managements.Data.Permissions;
using Managements.Data.Roles;
using Managements.Data.Scopes;
using Managements.Data.Services;
using Managements.Data.Users;
using Redis.OM;

namespace Managements.Data;

internal sealed class AuthorizeService : IAuthorizeService
{
    private readonly RedisConnectionProvider _dbProvider;

    public AuthorizeService(RedisConnectionProvider dbProvider)
    {
        _dbProvider = dbProvider;
    }

    public async Task<Guid> AuthorizeAsync(string token, string action, string serviceSecret, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _dbProvider.RedisCollection<SessionDataModel>(false).FindByIdAsync(token);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }

        if (Expired())
        {
            throw new SessionExpiredException();
        }

        var user = await _dbProvider.RedisCollection<UserDataModel>(false).FindByIdAsync(session.UserId.ToString());
        if (InvalidUser())
        {
            throw new UnAuthorizedException();
        }

        var roles = await RolesAsync(user!);

        var sessionScope = await _dbProvider.RedisCollection<ScopeDataModel>(false)
            .FindByIdAsync(session.ScopeId.ToString());
        if (InvalidSessionScope())
        {
            throw new UnAuthorizedException();
        }

        var service = await _dbProvider.RedisCollection<ServiceDataModel>(false).Where(model => model.Secret == serviceSecret)
            .FirstOrDefaultAsync();
        if (InvalidService())
        {
            if (service is null || service.Name != EntityBaseValues.KunderaServiceName)
            {
                throw new UnAuthorizedException();
            }
        }

        var permissionIds = roles.DistinctBy(role => role.Id)
            .Where(role => role.Permissions is not null)
            .SelectMany(role => role.Permissions!)
            .Distinct();
        var permissions = (await _dbProvider.RedisCollection<PermissionDataModel>(false)
            .FindByIdsAsync(permissionIds.Select(guid => guid.ToString()))).Values;
        if (InvalidPermission())
        {
            throw new UnAuthorizedException();
        }

        // await _eventBus.PublishAsync(new OnAuthorizeEvent(user!.Id.Value,
        //     sessionScope!.Id.Value,
        //     service!.Id.Value,
        //     action,
        //     ipAddress ?? IPAddress.None), cancellationToken);

        return user!.Id;

        bool Expired() => DateTime.UtcNow >= session.ExpiresAt;

        bool InvalidUser() => user is null || user.Status != UserStatus.Active.Name;

        bool InvalidSessionScope() => sessionScope is null || UserHasNotScopeRole();

        bool UserHasNotScopeRole() => sessionScope.Roles is null || !roles.Any(role => sessionScope.Roles.Any(id => id == role.Id));

        bool InvalidService() => service is null || sessionScope!.Services is null || sessionScope.Services.All(id => id != service.Id);

        bool InvalidPermission() => permissions.All(permission => permission is not null && permission.Name != action.ToLower());
    }

    private async Task<ICollection<RoleDataModel>> RolesAsync(UserDataModel user)
    {
        var userGroupRoleIds = await UserGroupsRolesAsync();
        var allRoleIds = new List<Guid>(userGroupRoleIds);
        if (user.Roles is not null)
        {
            allRoleIds.AddRange(user.Roles);
        }

        return (await _dbProvider.RedisCollection<RoleDataModel>()
                .FindByIdsAsync(allRoleIds.Select(guid => guid.ToString())))
            .Values!;

        async Task<IEnumerable<Guid>> UserGroupsRolesAsync()
        {
            var groupsDbCollection = _dbProvider.RedisCollection<GroupDataModel>(false);
            var groups = new List<GroupDataModel>();
            var currentUserGroups = (await groupsDbCollection.FindByIdsAsync(user.Groups.Select(guid => guid.ToString()))).Values;
            groups.AddRange(currentUserGroups!);
            foreach (var groupId in user.Groups)
            {
                var children = await FindChildrenAsync(groupId);
                groups.AddRange(children);
            }

            return groups.SelectMany(group => group.Roles);

            async Task<IEnumerable<GroupDataModel>> FindChildrenAsync(Guid id)
            {
                var currentGroup = await groupsDbCollection.FindByIdAsync(id.ToString());
                if (currentGroup is null) return Array.Empty<GroupDataModel>();
                var dataModels = new List<GroupDataModel>();
                await FetchChildrenAsync(currentGroup);

                return dataModels;

                async Task FetchChildrenAsync(GroupDataModel group)
                {
                    if (group.Children is {Count: 0}) return;
                    var ids = group.Children!.Select(groupId => groupId.ToString()).ToArray();
                    var children = (await groupsDbCollection.FindByIdsAsync(ids)).Values;
                    dataModels.AddRange(children!);
                    foreach (var groupDataModel in children)
                    {
                        await FetchChildrenAsync(groupDataModel!);
                    }
                }
            }
        }
    }
}