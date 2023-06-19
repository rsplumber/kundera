namespace Core.Groups.Events;

public sealed record GroupChildAddedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera.groups.child.added";
}

public sealed record GroupChildRemovedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera.groups.child.removed";
}