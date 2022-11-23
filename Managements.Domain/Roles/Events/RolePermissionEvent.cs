using Managements.Domain.Contracts;
using Managements.Domain.Permissions.Types;
using Managements.Domain.Roles.Types;

namespace Managements.Domain.Roles.Events;

[Event("role_permission_added")]
public sealed record RolePermissionAddedEvent(RoleId Id, PermissionId Permission) : DomainEvent;

[Event("role_permission_removed")]
public sealed record RolePermissionRemovedEvent(RoleId Id, PermissionId Permission) : DomainEvent;