using Mediator;
using Queries;
using Queries.Groups;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Groups;

public sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, PageableResponse<GroupsResponse>>
{
    private readonly IRedisCollection<GroupDataModel> _groups;

    public GroupsQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>)provider.RedisCollection<GroupDataModel>(false);
    }

    public async ValueTask<PageableResponse<GroupsResponse>> Handle(GroupsQuery query, CancellationToken cancellationToken)
    {
        var groupsQuery = await _groups.Page(query).ToListAsync();

        var groups = groupsQuery.Select(model => new GroupsResponse(model.Id, model.Name, model.Status)
            {
                Description = model.Description,
                Parent = model.Parent,
            })
            .ToList();

        var counts = await _groups.CountAsync();
        return new PageableResponse<GroupsResponse>
        {
            Data = groups,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}