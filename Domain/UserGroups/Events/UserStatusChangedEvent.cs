using Domain.UserGroups.Types;

namespace Domain.UserGroups.Events;

public record UserGroupStatusChangedEvent(UserGroupId UserGroupId, UserGroupStatus UserGroupStatus) : DomainEvent;