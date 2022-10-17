using Auth.Core.Domains;

namespace Auth.Core.Events;

public sealed record SessionCreatedEvent(Token Token) : DomainEvent;