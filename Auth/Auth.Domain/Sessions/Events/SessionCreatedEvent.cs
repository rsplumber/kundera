using Auth.Domain.Credentials;

namespace Auth.Domain.Sessions.Events;

public record SessionCreatedEvent(Token Token) : DomainEvent;