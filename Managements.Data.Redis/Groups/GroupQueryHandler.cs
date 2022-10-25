using Kite.CQRS;
using Managements.Application.Groups;
using Managements.Domain.Groups.Exception;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupQueryHandler : IQueryHandler<GroupQuery, GroupResponse>
{
    private readonly IRedisCollection<GroupDataModel> _groups;

    public GroupQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
    }

    public async Task<GroupResponse> HandleAsync(GroupQuery message, CancellationToken cancellationToken = default)
    {
        var groups = await _groups.FindByIdAsync(message.Id.Value.ToString());
        if (groups is null)
        {
            throw new GroupNotFoundException();
        }

        return new GroupResponse(groups.Id, groups.Name, groups.Status)
        {
            Description = groups.Description,
            Parent = groups.Parent,
            Roles = groups.Roles,
            StatusChangedDate = groups.StatusChangeDate
        };
    }
}