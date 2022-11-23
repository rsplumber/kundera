using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;
using Managements.Domain.Roles.Types;

namespace Managements.Domain.Groups.Events;

[Event("group_role_added")]
public sealed record GroupRoleAddedEvent(GroupId Id, RoleId Role) : DomainEvent;

[Event("group_role_removed")]
public sealed record GroupRoleRemovedEvent(GroupId Id, RoleId Role) : DomainEvent;