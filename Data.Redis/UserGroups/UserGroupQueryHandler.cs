using Application.UserGroups;
using Domain.UserGroups.Exception;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.UserGroups;

internal sealed class UserGroupQueryHandler : IQueryHandler<UserGroupQuery, UserGroupResponse>
{
    private readonly IRedisCollection<UserGroupDataModel> _userGroups;

    public UserGroupQueryHandler(RedisConnectionProvider provider)
    {
        _userGroups = (RedisCollection<UserGroupDataModel>) provider.RedisCollection<UserGroupDataModel>();
    }

    public async ValueTask<UserGroupResponse> HandleAsync(UserGroupQuery message, CancellationToken cancellationToken = default)
    {
        var userGroup = await _userGroups.FindByIdAsync(message.UserGroup.Value.ToString());
        if (userGroup is null)
        {
            throw new UserGroupNotFoundException();
        }

        return new UserGroupResponse(userGroup.Id, userGroup.Name, userGroup.Status)
        {
            Description = userGroup.Description,
            Parent = userGroup.Parent,
            Roles = userGroup.Roles,
            StatusChangedDate = userGroup.StatusChangedDate
        };
    }
}