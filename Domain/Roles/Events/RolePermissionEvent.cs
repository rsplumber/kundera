using Domain.Permissions;

namespace Domain.Roles.Events;

public record RolePermissionAddedEvent(RoleId Id, PermissionId Permission) : DomainEvent;

public record RolePermissionRemovedEvent(RoleId Id, PermissionId Permission) : DomainEvent;