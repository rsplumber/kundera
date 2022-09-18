using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record PermissionsQuery : Query<PermissionResponse>
{
    public Name? Name { get; set; }
}

public sealed record PermissionsResponse(string Id);