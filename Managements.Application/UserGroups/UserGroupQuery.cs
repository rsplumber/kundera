using Kite.CQRS.Contracts;
using Managements.Domain.UserGroups;

namespace Managements.Application.UserGroups;

public sealed record UserGroupQuery(UserGroupId UserGroup) : Query<UserGroupResponse>;

public sealed record UserGroupResponse(Guid Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public IEnumerable<Guid> Roles { get; set; }
}