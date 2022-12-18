using Mediator;

namespace Application.Scopes;

public sealed record ScopeQuery : IQuery<ScopeResponse>
{
    public Guid ScopeId { get; init; } = default!;
}

public sealed record ScopeResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Secret { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public IEnumerable<Guid>? Roles { get; init; }

    public IEnumerable<Guid>? Services { get; init; }
}