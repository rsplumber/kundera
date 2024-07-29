using Mediator;

namespace Data.Abstractions.Users;

public sealed record UsersRolesQuery : IQuery<List<UserRoleResponse>>
{
    public Guid UserId { get; set; }
}

public sealed record UserRoleResponse
{
    public string Role { get; set; } = default!;

    public List<string> Permissions { get; set; } = [];
}