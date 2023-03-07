namespace Core.Domains.Services.Events;

public sealed record ServiceStatusChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_services.status_changed";
}