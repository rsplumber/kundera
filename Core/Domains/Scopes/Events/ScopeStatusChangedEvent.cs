namespace Core.Domains.Scopes.Events;

public sealed record ScopeStatusChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_scopes.status_changed";
}