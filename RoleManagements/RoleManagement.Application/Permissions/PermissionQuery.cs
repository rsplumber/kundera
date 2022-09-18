using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record PermissionQuery(PermissionId PermissionId) : Query<PermissionResponse>;

public sealed record PermissionResponse(string Id)
{
    public Dictionary<string, string>? Meta { get; set; }
}

