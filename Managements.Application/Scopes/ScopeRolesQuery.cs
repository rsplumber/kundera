using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record ScopeRolesQuery(ScopeId Id) : Query<IEnumerable<ScopeRolesResponse>>
{
}

public sealed record ScopeRolesResponse(Guid Id, string Name);