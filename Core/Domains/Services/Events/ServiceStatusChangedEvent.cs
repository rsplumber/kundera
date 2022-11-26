using Core.Domains.Contracts;
using Core.Domains.Services.Types;

namespace Core.Domains.Services.Events;

[Event("service_status_changed")]
public sealed record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent;