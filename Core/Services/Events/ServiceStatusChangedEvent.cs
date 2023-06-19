namespace Core.Services.Events;

public sealed record ServiceStatusChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.services.status.changed";
}