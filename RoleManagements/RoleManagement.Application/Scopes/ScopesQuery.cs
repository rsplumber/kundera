using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record ScopesQuery : Query<IEnumerable<ScopesResponse>>
{
    public string? Name { get; set; }
}

public sealed record ScopesResponse(string Id, string Status);