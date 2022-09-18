using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ScopeQuery(ScopeId ScopeId) : Query<ScopeResponse>;

public sealed record ScopeResponse(string Id, string Status)
{
    public List<string>? Roles { get; set; }

    public List<string>? Services { get; set; }
}