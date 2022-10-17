namespace Managements.Domain.Services.Events;

public record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent;