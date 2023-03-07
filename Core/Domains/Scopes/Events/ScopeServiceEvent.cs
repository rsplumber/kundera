namespace Core.Domains.Scopes.Events;

public sealed record ScopeServiceAddedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera_scopes.service.added";
}

public sealed record ScopeServiceRemovedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera_scopes.service.removed";
}