using Core.Domains.Groups.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

public sealed record UserJoinedGroupEvent(UserId Id, GroupId Group) : DomainEvent
{
    public override string Name => "user_joined_group";
}

public sealed record UserRemovedGroupEvent(UserId Id, GroupId Group) : DomainEvent
{
    public override string Name => "user_removed_group";
}