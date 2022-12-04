using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

public sealed record GroupChildAddedEvent(GroupId GroupId, GroupId Child) : DomainEvent
{
    public override string Name => "group_child_added";
}

public sealed record GroupChildRemovedEvent(GroupId GroupId, GroupId Child) : DomainEvent
{
    public override string Name => "group_child_added";
}