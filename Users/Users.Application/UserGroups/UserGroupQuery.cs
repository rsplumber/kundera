using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.UserGroups;

public sealed record UserGroupQuery(UserGroupId UserGroup) : Query<UserGroupResponse>;

public sealed record UserGroupResponse(Guid Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public IEnumerable<string> Roles { get; set; }
}