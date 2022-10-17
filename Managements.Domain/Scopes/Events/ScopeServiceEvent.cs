using Managements.Domain.Services;

namespace Managements.Domain.Scopes.Events;

public record ScopeServiceAddedEvent(ScopeId Id, ServiceId Service) : DomainEvent;

public record ScopeServiceRemovedEvent(ScopeId Id, ServiceId Service) : DomainEvent;