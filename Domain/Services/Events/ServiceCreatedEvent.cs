using Domain.Services.Types;

namespace Domain.Services.Events;

public record ServiceCreatedEvent(ServiceId Id) : DomainEvent;