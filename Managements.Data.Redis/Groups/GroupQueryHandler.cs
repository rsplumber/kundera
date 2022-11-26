using Managements.Application.Groups;
using Managements.Data.Roles;
using Managements.Domain.Groups.Exception;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupQueryHandler : IQueryHandler<GroupQuery, GroupResponse>
{
    private readonly IRedisCollection<GroupDataModel> _groups;
    private readonly IRedisCollection<RoleDataModel> _roles;

    public GroupQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
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
            Roles = roles.Values.Select(model => new GroupRoleResponse(model.Id, model.Name)),
            StatusChangedDate = group.StatusChangeDate
        };
    }
}