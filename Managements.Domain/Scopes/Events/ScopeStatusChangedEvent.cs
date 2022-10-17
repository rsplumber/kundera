namespace Managements.Domain.Scopes.Events;

public record ScopeStatusChangedEvent(ScopeId Id) : DomainEvent;