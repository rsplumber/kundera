using RoleManagements.Domain.Roles.Types;

namespace RoleManagements.Domain.Roles.Events;

public record RoleCreatedEvent(RoleId Id) : DomainEvent;