using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Services.Events;

public record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent;