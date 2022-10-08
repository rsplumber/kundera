using Domain.Services;

namespace Domain.Scopes.Events;

public record ScopeServiceAddedEvent(ScopeId Id, ServiceId Service) : DomainEvent;

public record ScopeServiceRemovedEvent(ScopeId Id, ServiceId Service) : DomainEvent;