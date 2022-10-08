namespace Domain.Roles.Events;

public record RoleCreatedEvent(RoleId Id) : DomainEvent;