namespace Domain.UserGroups.Events;

public record UserGroupParentChangedEvent(UserGroupId GroupId, UserGroupId? Parent, UserGroupId? PreviousParent) : DomainEvent;

public record UserGroupParentMovedEvent(UserGroupId GroupId, UserGroupId Parent, UserGroupId? PreviousParent) : DomainEvent;