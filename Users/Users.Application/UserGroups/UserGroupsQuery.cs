using Tes.CQRS.Contracts;

namespace Users.Application.UserGroups;

public sealed record UserGroupsQuery() : Query<IEnumerable<UserGroupsResponse>>;

public sealed record UserGroupsResponse(Guid Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }
}