using Application.Groups;
using Core.Domains.Groups.Exception;
using Managements.Data.Roles;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupQueryHandler : IQueryHandler<GroupQuery, GroupResponse>
{
    private readonly IRedisCollection<GroupDataModel> _groups;
    private readonly IRedisCollection<RoleDataModel> _roles;

    public GroupQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>(false);
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>(false);
    }

    public async ValueTask<GroupResponse> Handle(GroupQuery query, CancellationToken cancellationToken)
    {
        var group = await _groups.FindByIdAsync(query.Id.ToString());
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var roles = await _roles.FindByIdsAsync(group.Roles.Select(guid => guid.ToString()));
        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Status = group.Status,
            Description = group.Description,
            Parent = group.Parent,
            Roles = roles.Values.Where(model => model is not null).Select(model => new GroupRoleResponse
            {
                Id = model!.Id,
                Name = model.Name
            }),
            StatusChangedDate = group.StatusChangeDate
        };
    }
}