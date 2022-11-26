using Core.Domains.Contracts;
using Core.Domains.Permissions.Types;
using Core.Domains.Roles.Types;

namespace Core.Domains.Roles.Events;

[Event("role_permission_added")]
public sealed record RolePermissionAddedEvent(RoleId Id, PermissionId Permission) : DomainEvent;

[Event("role_permission_removed")]
public sealed record RolePermissionRemovedEvent(RoleId Id, PermissionId Permission) : DomainEvent;