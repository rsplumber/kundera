using Managements.Domain.Roles;

namespace Managements.Domain.Groups.Events;

public record GroupRoleAddedEvent(GroupId Id, RoleId Role) : DomainEvent;

public record GroupRoleRemovedEvent(GroupId Id, RoleId Role) : DomainEvent;