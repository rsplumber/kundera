using Managements.Domain.Permissions;

namespace Managements.Domain.Roles.Events;

public record RolePermissionAddedEvent(RoleId Id, PermissionId Permission) : DomainEvent;

public record RolePermissionRemovedEvent(RoleId Id, PermissionId Permission) : DomainEvent;