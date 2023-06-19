namespace Core.Scopes.Events;

public sealed record ScopeServiceAddedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera.scopes.service.added";
}

public sealed record ScopeServiceRemovedEvent(Guid Id, Guid Service) : DomainEvent
{
    public override string Name => "kundera.scopes.service.removed";
}