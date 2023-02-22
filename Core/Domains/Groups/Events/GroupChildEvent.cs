namespace Core.Domains.Groups.Events;

public sealed record GroupChildAddedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera_group_child_added";
}

public sealed record GroupChildRemovedEvent(Guid GroupId, Guid Child) : DomainEvent
{
    public override string Name => "kundera_group_child_added";
}