using Domain.Roles;

namespace Domain.UserGroups.Events;

public record UserGroupRoleAddedEvent(UserGroupId Id, RoleId Role) : DomainEvent;

public record UserGroupRoleRemovedEvent(UserGroupId Id, RoleId Role) : DomainEvent;