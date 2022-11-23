using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

[Event("user_joined_group")]
public sealed record UserJoinedGroupEvent(UserId Id, GroupId Group) : DomainEvent;

[Event("user_removed_group")]
public sealed record UserRemovedGroupEvent(UserId Id, GroupId Group) : DomainEvent;