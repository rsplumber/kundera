using Mediator;

namespace Application.Scopes;

public sealed record ScopesQuery : IQuery<IEnumerable<ScopesResponse>>
{
    public string? Name { get; init; }
}

public sealed record ScopesResponse(Guid Id, string Name, string Status);