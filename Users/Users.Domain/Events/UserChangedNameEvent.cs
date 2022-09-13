namespace Users.Domain.Events;

public record UserChangedNameEvent(UserId UserId) : DomainEvent;