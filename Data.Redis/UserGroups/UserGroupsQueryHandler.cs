using Application.UserGroups;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.UserGroups;

internal sealed class UserGroupsQueryHandler : QueryHandler<UserGroupsQuery, IEnumerable<UserGroupsResponse>>
{
    private readonly IRedisCollection<UserGroupDataModel> _userGroups;

    public UserGroupsQueryHandler(RedisConnectionProvider provider)
    {
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public override async Task<IEnumerable<UserGroupsResponse>> HandleAsync(UserGroupsQuery message, CancellationToken cancellationToken = default)
    {
        var groups = await _userGroups.ToListAsync();
        return groups.Select(model => new UserGroupsResponse(model.Id, model.Name, model.Status)
        {
            Description = model.Description,
            Parent = model.Parent,
        }).ToList();
    }
}