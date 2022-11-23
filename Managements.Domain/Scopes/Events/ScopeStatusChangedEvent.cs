using Managements.Domain.Contracts;
using Managements.Domain.Scopes.Types;

namespace Managements.Domain.Scopes.Events;

[Event("scope_status_changed")]
public sealed record ScopeStatusChangedEvent(ScopeId Id) : DomainEvent;