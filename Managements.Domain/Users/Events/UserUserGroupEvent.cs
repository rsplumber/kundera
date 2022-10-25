using Managements.Domain.Groups;

namespace Managements.Domain.Users.Events;

public record UserJoinedGroupEvent(UserId Id, GroupId Group) : DomainEvent;

public record UserRemovedGroupEvent(UserId Id, GroupId Group) : DomainEvent;