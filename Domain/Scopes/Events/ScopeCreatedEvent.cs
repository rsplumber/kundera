using Domain.Scopes.Types;

namespace Domain.Scopes.Events;

public record ScopeCreatedEvent(ScopeId Id) : DomainEvent;