using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ScopesQuery(ScopeId ScopeId) : Query<ScopesResponse>;

public sealed record ScopesResponse(string Id, string Status);