using Core.Domains.Contracts;
using Core.Domains.Groups.Types;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

[Event("user_joined_group")]
public sealed record UserJoinedGroupEvent(UserId Id, GroupId Group) : DomainEvent;

[Event("user_removed_group")]
public sealed record UserRemovedGroupEvent(UserId Id, GroupId Group) : DomainEvent;