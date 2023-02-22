namespace Core.Domains.Scopes.Events;

public sealed record ScopeServiceAddedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera_scope_service_added";
}

public sealed record ScopeServiceRemovedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera_scope_service_removed";
}