using Core.Domains.Contracts;
using Core.Domains.Groups.Types;
using Core.Domains.Roles.Types;

namespace Core.Domains.Groups.Events;

[Event("group_role_added")]
public sealed record GroupRoleAddedEvent(GroupId Id, RoleId Role) : DomainEvent;

[Event("group_role_removed")]
public sealed record GroupRoleRemovedEvent(GroupId Id, RoleId Role) : DomainEvent;