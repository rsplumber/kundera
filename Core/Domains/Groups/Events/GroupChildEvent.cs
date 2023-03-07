namespace Core.Domains.Groups.Events;

public sealed record GroupChildAddedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera_groups.child.added";
}

public sealed record GroupChildRemovedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera_groups.child.removed";
}