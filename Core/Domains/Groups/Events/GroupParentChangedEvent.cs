using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

public record GroupParentChangedEvent(GroupId GroupId, GroupId? Parent, GroupId? PreviousParent) : DomainEvent
{
    public override string Name => "group_parent_changed";
}