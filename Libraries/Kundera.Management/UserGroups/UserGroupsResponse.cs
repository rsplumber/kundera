namespace Kundera.Management.UserGroups;

public sealed record UserGroupsResponse(Guid Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }
}