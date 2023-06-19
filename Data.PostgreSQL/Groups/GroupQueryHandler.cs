using Core.Groups.Exception;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Queries.Groups;

namespace Data.Groups;

public sealed class GroupQueryHandler : IQueryHandler<GroupQuery, GroupResponse>
{
    private readonly AppDbContext _dbContext;

    public GroupQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<GroupResponse> Handle(GroupQuery query, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .AsNoTracking()
            .Include(g => g.Parent)
            .Include(g => g.Roles)
            .FirstOrDefaultAsync(g => g.Id == query.GroupId, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Status = group.Status.ToString(),
            Description = group.Description,
            Parent = group.Parent?.Id,
            StatusChangedDate = group.StatusChangeDateUtc,
            Roles = group.Roles.Select(model => new GroupRoleResponse
            {
                Id = model.Id,
                Name = model.Name
            })
        };
    }
}