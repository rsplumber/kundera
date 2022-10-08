using Domain.Scopes;
using Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record ScopeQuery(ScopeId Scope) : Query<ScopeResponse>;

public sealed record ScopeResponse(string Id, string Status)
{
    public IEnumerable<string>? Roles { get; set; }

    public IEnumerable<string>? Services { get; set; }
}