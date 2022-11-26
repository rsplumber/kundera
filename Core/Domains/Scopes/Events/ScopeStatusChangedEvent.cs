using Core.Domains.Contracts;
using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

[Event("scope_status_changed")]
public sealed record ScopeStatusChangedEvent(ScopeId Id) : DomainEvent;