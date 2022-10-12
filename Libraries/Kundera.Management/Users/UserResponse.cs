namespace Kundera.Management.Users;

public sealed record UserResponse(Guid Id, IEnumerable<string> Usernames)
{
    public string Status { get; set; }
    public IEnumerable<Guid> UserGroups { get; set; } = Array.Empty<Guid>();

    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}