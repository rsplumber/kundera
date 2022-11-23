using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups.Events;

[Event("group_child_added")]
public sealed record GroupChildAddedEvent(GroupId GroupId, GroupId Child) : DomainEvent;

[Event("group_child_removed")]
public sealed record GroupChildRemovedEvent(GroupId GroupId, GroupId Child) : DomainEvent;