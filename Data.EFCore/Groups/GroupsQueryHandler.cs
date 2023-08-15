using Data.Abstractions;
using Data.Abstractions.Groups;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Groups;

public sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, PageableResponse<GroupsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GroupsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PageableResponse<GroupsResponse>> Handle(GroupsQuery query, CancellationToken cancellationToken)
    {
        var groups = await _dbContext.Groups
            .AsNoTracking()
            .Page(query)
            .Select(model => new GroupsResponse(model.Id, model.Name, model.Status.ToString())
            {
                Description = model.Description,
                Parent = model.Parent != null ? model.Parent.Id : null
            })
            .ToListAsync(cancellationToken);

        var counts = await _dbContext.Groups.CountAsync(cancellationToken);

        return new PageableResponse<GroupsResponse>
        {
            Data = groups,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}