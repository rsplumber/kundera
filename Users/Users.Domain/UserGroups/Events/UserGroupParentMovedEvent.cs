using Users.Domain.Users.Events;

namespace Users.Domain.UserGroups.Events;

public record UserGroupParentMovedEvent(UserGroupId GroupId, UserGroupId Parent, UserGroupId? PreviousParent) : DomainEvent;