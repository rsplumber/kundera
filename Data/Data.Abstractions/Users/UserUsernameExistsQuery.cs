using Mediator;

namespace Data.Abstractions.Users;

public sealed record UserUsernameExistsQuery : IQuery<UserUsernameExistsResponse>
{
    public string Username { get; init; } = default!;
}

public sealed record UserUsernameExistsResponse
{
    public Guid Id { get; set; } = default!;
}