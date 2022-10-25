using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups.Events;

public record GroupStatusChangedEvent(GroupId GroupId, GroupStatus Status) : DomainEvent;