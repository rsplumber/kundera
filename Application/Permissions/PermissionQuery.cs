using Domain.Permissions;
using Tes.CQRS.Contracts;

namespace Application.Permissions;

public sealed record PermissionQuery(PermissionId Permission) : Query<PermissionResponse>;

public sealed record PermissionResponse(string Id)
{
    public Dictionary<string, string>? Meta { get; set; }
}