using Core.Domains.Services.Types;

namespace Core.Domains.Services.Events;

public sealed record ServiceCreatedEvent(ServiceId Id) : DomainEvent
{
    public override string Name => "service_created";
}