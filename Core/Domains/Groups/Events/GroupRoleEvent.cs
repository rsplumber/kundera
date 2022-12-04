using Core.Domains.Groups.Types;
using Core.Domains.Roles.Types;

namespace Core.Domains.Groups.Events;

public sealed record GroupRoleAddedEvent(GroupId Id, RoleId Role) : DomainEvent
{
    public override string Name => "group_role_added";
}

public sealed record GroupRoleRemovedEvent(GroupId Id, RoleId Role) : DomainEvent
{
    public override string Name => "group_role_removed";
}