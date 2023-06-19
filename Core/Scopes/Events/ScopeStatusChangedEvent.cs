namespace Core.Scopes.Events;

public sealed record ScopeStatusChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.scopes.status.changed";
}