using Core.Domains.Contracts;
using Core.Domains.Services.Types;

namespace Core.Domains.Services.Events;

[Event("service_created")]
public sealed record ServiceCreatedEvent(ServiceId Id) : DomainEvent;