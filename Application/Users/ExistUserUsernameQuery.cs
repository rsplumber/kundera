using Mediator;

namespace Application.Users;

public sealed record ExistUserUsernameQuery : IQuery<Guid>
{
    public string Username { get; init; } = default!;
}