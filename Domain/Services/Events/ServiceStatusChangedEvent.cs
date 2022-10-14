namespace Domain.Services.Events;

public record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent;