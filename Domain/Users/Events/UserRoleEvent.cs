using Domain.Roles;

namespace Domain.Users.Events;

public record UserRoleAddedEvent(UserId Id, RoleId Role) : DomainEvent;

public record UserRoleRemovedEvent(UserId Id, RoleId Role) : DomainEvent;