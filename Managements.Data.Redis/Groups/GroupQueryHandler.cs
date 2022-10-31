using Kite.CQRS;
using Managements.Application.Groups;
using Managements.Data.Roles;
using Managements.Domain.Groups.Exception;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Groups;

internal sealed class GroupQueryHandler : IQueryHandler<GroupQuery, GroupResponse>
{
    private readonly IRedisCollection<GroupDataModel> _groups;
    private readonly IRedisCollection<RoleDataModel> _roles;

    public GroupQueryHandler(RedisConnectionProvider provider)
    {
        _groups = (RedisCollection<GroupDataModel>) provider.RedisCollection<GroupDataModel>();
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async Task<GroupResponse> HandleAsync(GroupQuery message, CancellationToken cancellationToken = default)
    {
        var group = await _groups.FindByIdAsync(message.Id.Value.ToString());
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var roles = await _roles.FindByIdsAsync(group.Roles.Select(guid => guid.ToString()));
        return new GroupResponse(group.Id, group.Name, group.Status)
        {
            Description = group.Description,
            Parent = group.Parent,
            Roles = roles.Values.Select(model => new GroupRoleResponse(model.Id, model.Name)),
            StatusChangedDate = group.StatusChangeDate
        };
    }
}