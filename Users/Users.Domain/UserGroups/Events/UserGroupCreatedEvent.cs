using Users.Domain.Users.Events;

namespace Users.Domain.UserGroups.Events;

public record UserGroupCreatedEvent(UserGroupId GroupId) : DomainEvent;