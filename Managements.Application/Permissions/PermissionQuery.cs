using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Permissions;

namespace Managements.Application.Permissions;

public sealed record PermissionQuery(PermissionId Permission) : Query<PermissionResponse>;

public sealed record PermissionResponse(Guid Id, Name Name)
{
    public Dictionary<string, string>? Meta { get; set; }
}