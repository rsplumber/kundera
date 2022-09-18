using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record PermissionsQuery : Query<IEnumerable<PermissionResponse>>
{
    public string? Name { get; set; }
}

public sealed record PermissionsResponse(string Id);