using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

public sealed record GroupStatusChangedEvent(GroupId GroupId, GroupStatus Status) : DomainEvent
{
    public override string Name => "group_status_changed";
}