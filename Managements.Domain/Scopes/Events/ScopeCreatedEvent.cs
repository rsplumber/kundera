using Managements.Domain.Contracts;
using Managements.Domain.Scopes.Types;

namespace Managements.Domain.Scopes.Events;

[Event("scope_created")]
public sealed record ScopeCreatedEvent(ScopeId Id) : DomainEvent;