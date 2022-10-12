namespace Kundera.Management.Roles;

public sealed record RoleResponse(string Id)
{
    public IEnumerable<string>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}