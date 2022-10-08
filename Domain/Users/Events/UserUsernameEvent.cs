namespace Domain.Users.Events;

public record UserUsernameAddedEvent(UserId Id, Username Username) : DomainEvent;

public record UserUsernameRemovedEvent(UserId Id, Username Username) : DomainEvent;