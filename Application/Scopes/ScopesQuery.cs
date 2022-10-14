using Kite.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ScopesQuery : Query<IEnumerable<ScopesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ScopesResponse(string Id, string Status);