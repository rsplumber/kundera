using Core.Domains.Services.Types;

namespace Core.Domains.Services.Events;

public sealed record ServiceStatusChangedEvent(ServiceId Id) : DomainEvent
{
    public override string Name => "service_status_changed";
}