using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

public sealed record ScopeStatusChangedEvent(ScopeId Id) : DomainEvent
{
    public override string Name => "scope_status_changed";
}