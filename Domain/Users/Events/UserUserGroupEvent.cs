using Domain.UserGroups;

namespace Domain.Users.Events;

public record UserUserGroupJoinedEvent(UserId Id, UserGroupId UserGroup) : DomainEvent;

public record UserUserGroupEventRemovedEvent(UserId Id, UserGroupId UserGroup) : DomainEvent;