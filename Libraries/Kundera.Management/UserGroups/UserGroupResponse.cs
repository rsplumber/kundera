namespace Kundera.Management.UserGroups;

public sealed record UserGroupResponse(Guid Id, string Name, string UserGroupStatus)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public IEnumerable<string>? Roles { get; set; }
}