using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record UserQuery(UserId User) : Query<UserResponse>;

public sealed record UserResponse(Guid Id, IEnumerable<string> Usernames)
{
    public IEnumerable<string> UserGroups { get; set; } = Array.Empty<string>();

    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
}