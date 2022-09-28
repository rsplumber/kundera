using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record RoleQuery(RoleId RoleId) : Query<RoleResponse>;

public sealed record RoleResponse(string Id)
{
    public IEnumerable<string>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}