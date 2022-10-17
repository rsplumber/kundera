using Managements.Domain.Roles;

namespace Managements.Domain.UserGroups.Events;

public record UserGroupRoleAddedEvent(UserGroupId Id, RoleId Role) : DomainEvent;

public record UserGroupRoleRemovedEvent(UserGroupId Id, RoleId Role) : DomainEvent;