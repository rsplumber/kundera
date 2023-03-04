using AutoMapper;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Core.Domains.Roles;
using Core.Domains.Services;
using Core.Domains.Users;
using Data.Auth.Sessions;
using Data.Groups;
using Data.Roles;
using Data.Services;
using Redis.OM;

namespace Data.Auth;

internal sealed class AuthorizeDataProvider : IAuthorizeDataProvider
{
    private readonly RedisConnectionProvider _dbProvider;
    private readonly IMapper _mapper;

    public AuthorizeDataProvider(RedisConnectionProvider dbProvider, IMapper mapper)
    {
        _dbProvider = dbProvider;
        _mapper = mapper;
    }

    public async Task<Session?> CurrentSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        var dataModel = await _dbProvider.RedisCollection<SessionDataModel>(false).FindByIdAsync(sessionToken);
        return dataModel is null ? null : _mapper.Map<Session>(dataModel);
    }

    public async Task<List<Role>> UserRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var userGroupRoleIds = await UserGroupsRolesAsync();
        var allRoleIds = new List<Guid>(userGroupRoleIds);
        allRoleIds.AddRange(user.Roles.Select(r => r.Id));

        var roleDataModels = await _dbProvider.RedisCollection<RoleDataModel>(false)
            .FindByIdsAsync(allRoleIds.Select(guid => guid.ToString()));
        return roleDataModels.Values.Select(model => _mapper.Map<Role>(model)).ToList();

        async Task<IEnumerable<Guid>> UserGroupsRolesAsync()
        {
            var groupsDbCollection = _dbProvider.RedisCollection<GroupDataModel>(false);
            var groups = new List<GroupDataModel>();
            var currentUserGroups = (await groupsDbCollection.FindByIdsAsync(user.Groups.Select(guid => guid.ToString()))).Values;
            groups.AddRange(currentUserGroups!);
            foreach (var group in user.Groups)
            {
                var children = await FindChildrenAsync(group.Id);
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
                    if (group.Children is { Count: 0 }) return;
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

    public async Task<Service?> RequestedServiceAsync(string serviceSecret, CancellationToken cancellationToken = default)
    {
        var dataModel = await _dbProvider.RedisCollection<ServiceDataModel>(false).Where(model => model.Secret == serviceSecret)
            .FirstOrDefaultAsync();

        return dataModel is null ? null : _mapper.Map<Service>(dataModel);
    }
}