using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Services.Events;

public record ServiceCreatedEvent(ServiceId Id) : DomainEvent;