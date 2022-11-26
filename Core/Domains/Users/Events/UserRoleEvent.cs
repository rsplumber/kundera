using Core.Domains.Contracts;
using Core.Domains.Roles.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

[Event("user_role_added")]
public sealed record UserRoleAddedEvent(UserId Id, RoleId Role) : DomainEvent;

[Event("user_role_removed")]
public sealed record UserRoleRemovedEvent(UserId Id, RoleId Role) : DomainEvent;