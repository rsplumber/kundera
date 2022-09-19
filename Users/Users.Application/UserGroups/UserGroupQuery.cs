using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.UserGroups;

public sealed record UserGroupQuery(UserGroupId UserGroup) : Query<UserGroupResponse>;

public sealed record UserGroupResponse(string Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public string? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public List<string> Roles { get; set; }
}