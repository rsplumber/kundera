using Kite.CQRS.Contracts;
using Managements.Domain.Users;

namespace Managements.Application.Users;

public sealed record UserQuery(UserId User) : Query<UserResponse>;

public sealed record UserResponse(Guid Id, IEnumerable<string> Usernames)
{
    public string Status { get; set; }

    public IEnumerable<Guid> Groups { get; set; } = Array.Empty<Guid>();

    public IEnumerable<Guid> Roles { get; set; } = Array.Empty<Guid>();
}