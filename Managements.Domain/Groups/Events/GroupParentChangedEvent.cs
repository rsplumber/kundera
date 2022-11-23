using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups.Events;

[Event("group_parent_changed")]
public record GroupParentChangedEvent(GroupId GroupId, GroupId? Parent, GroupId? PreviousParent) : DomainEvent;