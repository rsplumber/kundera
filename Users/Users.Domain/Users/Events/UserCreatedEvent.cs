namespace Users.Domain.Events;

public record UserCreatedEvent(UserId UserId) : DomainEvent;