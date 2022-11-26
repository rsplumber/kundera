using Core.Domains.Contracts;
using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

[Event("group_parent_changed")]
public record GroupParentChangedEvent(GroupId GroupId, GroupId? Parent, GroupId? PreviousParent) : DomainEvent;