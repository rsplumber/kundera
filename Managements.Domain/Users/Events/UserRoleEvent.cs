using Managements.Domain.Contracts;
using Managements.Domain.Roles.Types;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

[Event("user_role_added")]
public sealed record UserRoleAddedEvent(UserId Id, RoleId Role) : DomainEvent;

[Event("user_role_removed")]
public sealed record UserRoleRemovedEvent(UserId Id, RoleId Role) : DomainEvent;