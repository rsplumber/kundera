using Mediator;

namespace Data.Abstractions.Users;

public sealed record UserUsernameCredentialQuery : IQuery<UserUsernameCredentialQueryResponse>
{
    public string Username { get; init; } = default!;
}

public sealed record UserUsernameCredentialQueryResponse
{
    public Guid Id { get; set; } = default!;

    public string Username { get; set; } = default!;

    public Guid UserId { get; set; } = default!;
}