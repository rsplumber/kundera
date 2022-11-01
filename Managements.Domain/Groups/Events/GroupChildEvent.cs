namespace Managements.Domain.Groups.Events;

public record GroupChildAddedEvent(GroupId GroupId, GroupId Child) : DomainEvent;

public record GroupChildRemovedEvent(GroupId GroupId, GroupId Child) : DomainEvent;