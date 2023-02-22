namespace Core.Domains.Scopes.Events;

public sealed record ScopeStatusChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_scope_status_changed";
}