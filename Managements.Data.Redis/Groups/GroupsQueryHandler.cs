using Kite.CQRS;
using Managements.Application.Groups;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, IEnumerable<GroupsResponse>>
{
    private readonly IRedisCollection<GroupDataModel> _groups;

    public GroupsQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
    }

    public async Task<IEnumerable<GroupsResponse>> HandleAsync(GroupsQuery message, CancellationToken cancellationToken = default)
    {
        var groups = await _groups.ToListAsync();

        return groups.Select(model => new GroupsResponse(model.Id, model.Name, model.Status)
            {
                Description = model.Description,
                Parent = model.Parent,
            })
            .ToList();
    }
}