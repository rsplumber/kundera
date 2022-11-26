using Core.Domains.Contracts;
using Core.Domains.Scopes.Types;
using Core.Domains.Services.Types;

namespace Core.Domains.Scopes.Events;

[Event("scope_service_added")]
public sealed record ScopeServiceAddedEvent(ScopeId Id, ServiceId Service) : DomainEvent;

[Event("scope_service_removed")]
public sealed record ScopeServiceRemovedEvent(ScopeId Id, ServiceId Service) : DomainEvent;