using Mediator;

namespace Queries.Scopes;

public sealed record ScopeRolesQuery : IQuery<List<ScopeRolesResponse>>
{
    public Guid ScopeId { get; init; } = default!;
}

public sealed record ScopeRolesResponse(Guid Id, string Name);