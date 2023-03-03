using Mediator;

namespace Application.Users;

public sealed record ExistUserUsernameQuery : IQuery<bool>
{
    public string Username { get; init; } = default!;
}