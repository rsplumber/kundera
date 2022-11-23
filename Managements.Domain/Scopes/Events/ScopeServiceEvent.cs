using Managements.Domain.Contracts;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Scopes.Events;

[Event("scope_service_added")]
public sealed record ScopeServiceAddedEvent(ScopeId Id, ServiceId Service) : DomainEvent;

[Event("scope_service_removed")]
public sealed record ScopeServiceRemovedEvent(ScopeId Id, ServiceId Service) : DomainEvent;