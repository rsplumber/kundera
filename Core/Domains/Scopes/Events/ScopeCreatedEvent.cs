namespace Core.Domains.Scopes.Events;

public sealed record ScopeCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_scope_created";
}