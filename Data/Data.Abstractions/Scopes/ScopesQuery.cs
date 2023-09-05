using Mediator;

namespace Data.Abstractions.Scopes;

public sealed record ScopesQuery : PageableQuery, IQuery<PageableResponse<ScopesResponse>>
{
    public string? Name { get; init; }
}

public sealed record ScopesResponse(Guid Id, string Name, string Status);