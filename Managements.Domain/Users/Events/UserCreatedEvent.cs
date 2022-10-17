namespace Managements.Domain.Users.Events;

public record UserCreatedEvent(UserId UserId) : DomainEvent;