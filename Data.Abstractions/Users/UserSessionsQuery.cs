using Mediator;

namespace Data.Abstractions.Users;

public sealed record UserSessionsQuery : IQuery<List<UserSessionResponse>>
{
    public Guid UserId { get; init; }
}

public sealed record UserSessionResponse
{
    public string Id { get; set; } = default!;

    public Guid ScopeId { get; set; }

    public DateTime ExpirationDateUtc { get; set; }
}