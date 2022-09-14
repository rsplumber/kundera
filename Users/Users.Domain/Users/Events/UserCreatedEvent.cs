namespace Users.Domain.Users.Events;

public record UserCreatedEvent(UserId UserId) : DomainEvent;