using RoleManagements.Domain.Permissions.Types;

namespace RoleManagements.Domain.Permissions.Events;

public record PermissionCreatedEvent(PermissionId Id) : DomainEvent;