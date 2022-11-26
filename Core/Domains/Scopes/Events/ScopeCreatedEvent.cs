using Core.Domains.Contracts;
using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

[Event("scope_created")]
public sealed record ScopeCreatedEvent(ScopeId Id) : DomainEvent;