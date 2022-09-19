using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record PermissionsQuery : Query<IEnumerable<PermissionsResponse>>
{
    public string? Name { get; set; }
}

public sealed record PermissionsResponse(string Id);