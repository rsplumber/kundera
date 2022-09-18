using Users.Domain.UserGroups.Types;
using Users.Domain.Users.Events;

namespace Users.Domain.UserGroups.Events;

public record UserGroupStatusChangedEvent(UserGroupId UserGroupId, UserGroupStatus UserGroupStatus) : DomainEvent;