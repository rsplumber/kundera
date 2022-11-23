using Managements.Domain.Contracts;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services.Events;

[Event("service_created")]
public sealed record ServiceCreatedEvent(ServiceId Id) : DomainEvent;