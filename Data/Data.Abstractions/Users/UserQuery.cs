using Mediator;

namespace Data.Abstractions.Users;

public sealed record UserQuery : IQuery<UserResponse>
{
    public Guid UserId { get; init; } = default!;
}

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Status { get; init; } = string.Empty;

    public List<string> Usernames { get; init; } = Array.Empty<string>().ToList();

    public IEnumerable<Guid> Groups { get; init; } = Array.Empty<Guid>().ToList();

    public IEnumerable<Guid>? Roles { get; init; } = Array.Empty<Guid>().ToList();
}