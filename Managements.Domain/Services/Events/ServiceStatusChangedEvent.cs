using Managements.Domain.Contracts;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services.Events;

[Event("service_status_changed")]
public sealed record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent;