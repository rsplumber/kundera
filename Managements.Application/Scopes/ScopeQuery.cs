using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record ScopeQuery(ScopeId Scope) : Query<ScopeResponse>;

public sealed record ScopeResponse(Guid Id, string Name, string Secret, string Status)
{
    public IEnumerable<Guid>? Roles { get; set; }

    public IEnumerable<Guid>? Services { get; set; }
}