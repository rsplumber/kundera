namespace Domain.UserGroups.Events;

public record UserGroupCreatedEvent(UserGroupId GroupId) : DomainEvent;