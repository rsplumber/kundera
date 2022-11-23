using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups.Events;

[Event("group_status_changed")]
public sealed record GroupStatusChangedEvent(GroupId GroupId, GroupStatus Status) : DomainEvent;