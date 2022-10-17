namespace Managements.Domain.Services.Events;

public record ServiceCreatedEvent(ServiceId Id) : DomainEvent;