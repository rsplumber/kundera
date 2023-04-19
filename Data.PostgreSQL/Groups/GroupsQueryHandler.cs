using Application.Groups;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Data.Groups;

public sealed class GroupsQueryHandler : IQueryHandler<GroupsQuery, List<GroupsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GroupsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<List<GroupsResponse>> Handle(GroupsQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Select(model => new GroupsResponse(model.Id, model.Name, model.Status.ToString())
            {
                Description = model.Description,
                Parent = model.Parent != null ? model.Parent.Id : null
            })
            .ToListAsync(cancellationToken);
    }
}