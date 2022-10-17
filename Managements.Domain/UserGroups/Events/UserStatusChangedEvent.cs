using Managements.Domain.UserGroups.Types;

namespace Managements.Domain.UserGroups.Events;

public record UserGroupStatusChangedEvent(UserGroupId UserGroupId, UserGroupStatus UserGroupStatus) : DomainEvent;