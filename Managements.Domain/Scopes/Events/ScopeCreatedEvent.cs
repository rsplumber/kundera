namespace Managements.Domain.Scopes.Events;

public record ScopeCreatedEvent(ScopeId Id) : DomainEvent;