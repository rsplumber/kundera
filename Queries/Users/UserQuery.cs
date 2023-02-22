using Mediator;

namespace Queries.Users;

public sealed record UserQuery : IQuery<UserResponse>
{
    public Guid UserId { get; init; } = default!;
}

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Status { get; init; } = string.Empty;

    public List<string> Usernames { get; init; } = Array.Empty<string>().ToList();

    public List<Guid> Groups { get; init; } = Array.Empty<Guid>().ToList();

    public List<Guid>? Roles { get; init; } = Array.Empty<Guid>().ToList();
}