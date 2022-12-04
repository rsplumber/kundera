using Core.Domains.Roles.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

public sealed record UserRoleAddedEvent(UserId Id, RoleId Role) : DomainEvent
{
    public override string Name => "user_role_added";
}

public sealed record UserRoleRemovedEvent(UserId Id, RoleId Role) : DomainEvent
{
    public override string Name => "user_role_removed";
}