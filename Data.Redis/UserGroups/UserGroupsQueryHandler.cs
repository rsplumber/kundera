using Application.UserGroups;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.UserGroups;

internal sealed class UserGroupsIQueryHandler : IQueryHandler<UserGroupsQuery, IEnumerable<UserGroupsResponse>>
{
    private readonly IRedisCollection<UserGroupDataModel> _userGroups;

    public UserGroupsIQueryHandler(RedisConnectionProvider provider)
    {
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public async ValueTask<IEnumerable<UserGroupsResponse>> HandleAsync(UserGroupsQuery message, CancellationToken cancellationToken = default)
    {
        var groups = await _userGroups.ToListAsync();

        return groups.Select(model => new UserGroupsResponse(model.Id, model.Name, model.Status)
                     {
                         Description = model.Description,
                         Parent = model.Parent,
                     })
                     .ToList();
    }
}