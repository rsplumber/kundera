namespace Auth.Domain.Sessions.Events;

public sealed record SessionCreatedEvent(Token Token) : DomainEvent;