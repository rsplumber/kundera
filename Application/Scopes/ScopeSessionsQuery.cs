using Mediator;

namespace Application.Scopes;

public sealed record ScopeSessionsQuery : IQuery<List<ScopeSessionResponse>>
{
    public Guid ScopeId { get; init; }
}

public sealed record ScopeSessionResponse
{
    public string Id { get; set; } = default!;

    public Guid ScopeId { get; set; }

    public Guid UserId { get; set; }

    public DateTime ExpirationDateUtc { get; set; }

}