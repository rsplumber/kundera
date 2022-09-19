using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record UserQuery(UserId User) : Query<UserResponse>;

public sealed record UserResponse(string Id, string Username)
{
    public IEnumerable<string> UserGroups { get; set; } = Array.Empty<string>();

    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}