using Kite.CQRS;
using Managements.Application.UserGroups;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.UserGroups;

internal sealed class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, IEnumerable<UserGroupsResponse>>
{
    private readonly IRedisCollection<UserGroupDataModel> _userGroups;

    public UserGroupsQueryHandler(RedisConnectionProvider provider)
    {
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public async Task<IEnumerable<UserGroupsResponse>> HandleAsync(UserGroupsQuery message, CancellationToken cancellationToken = default)
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