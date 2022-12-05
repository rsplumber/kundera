using Application.Groups;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, List<GroupsResponse>>
{
    private readonly IRedisCollection<GroupDataModel> _groups;

    public GroupsQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>(false);
    }

    public async ValueTask<List<GroupsResponse>> Handle(GroupsQuery query, CancellationToken cancellationToken)
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