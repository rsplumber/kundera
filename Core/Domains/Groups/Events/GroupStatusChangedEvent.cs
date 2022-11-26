using Core.Domains.Contracts;
using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

[Event("group_status_changed")]
public sealed record GroupStatusChangedEvent(GroupId GroupId, GroupStatus Status) : DomainEvent;