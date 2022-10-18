using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record ScopeQuery(ScopeId Scope) : Query<ScopeResponse>;

public sealed record ScopeResponse(Guid Id, string Name, string Status)
{
    public IEnumerable<string>? Roles { get; set; }

    public IEnumerable<string>? Services { get; set; }
}