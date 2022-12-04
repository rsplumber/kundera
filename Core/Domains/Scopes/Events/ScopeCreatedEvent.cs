using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

public sealed record ScopeCreatedEvent(ScopeId Id) : DomainEvent
{
    public override string Name => "scope_created";
}