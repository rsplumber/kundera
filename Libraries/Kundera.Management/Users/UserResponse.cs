namespace Kundera.Management.Users;

public sealed record UserResponse(Guid Id, IEnumerable<string> Usernames, string Status)
{
    public IEnumerable<Guid> UserGroups { get; set; } = Array.Empty<Guid>();

    public IEnumerable<string>? Roles { get; set; } = Array.Empty<string>();
}