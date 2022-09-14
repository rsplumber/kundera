using Users.Domain.Users.Events;

namespace Users.Domain.UserGroups.Events;

public record UserGroupParentChangedEvent(UserGroupId GroupId, UserGroupId Parent, UserGroupId? PreviousParent) : DomainEvent;