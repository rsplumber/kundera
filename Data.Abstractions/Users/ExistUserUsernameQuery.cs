using Mediator;

namespace Data.Abstractions.Users;

public sealed record ExistUserUsernameQuery : IQuery<bool>
{
    public string Username { get; init; } = default!;
}