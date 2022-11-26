using Core.Domains.Contracts;
using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

[Event("group_child_added")]
public sealed record GroupChildAddedEvent(GroupId GroupId, GroupId Child) : DomainEvent;

[Event("group_child_removed")]
public sealed record GroupChildRemovedEvent(GroupId GroupId, GroupId Child) : DomainEvent;