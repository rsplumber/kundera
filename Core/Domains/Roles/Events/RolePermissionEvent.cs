using Core.Domains.Permissions.Types;
using Core.Domains.Roles.Types;

namespace Core.Domains.Roles.Events;

public sealed record RolePermissionAddedEvent(RoleId Id, PermissionId Permission) : DomainEvent
{
    public override string Name => "role_permission_added";
}

public sealed record RolePermissionRemovedEvent(RoleId Id, PermissionId Permission) : DomainEvent
{
    public override string Name => "role_permission_removed";
}