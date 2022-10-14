namespace Domain.Services.Events;

public record ServiceCreatedEvent(ServiceId Id) : DomainEvent;