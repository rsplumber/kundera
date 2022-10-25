namespace Managements.Domain.Groups.Events;

public record GroupCreatedEvent(GroupId GroupId) : DomainEvent;