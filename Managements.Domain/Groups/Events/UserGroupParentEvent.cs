namespace Managements.Domain.Groups.Events;

public record GroupParentChangedEvent(GroupId GroupId, GroupId? Parent, GroupId? PreviousParent) : DomainEvent;

public record GroupParentMovedEvent(GroupId GroupId, GroupId Parent, GroupId? PreviousParent) : DomainEvent;