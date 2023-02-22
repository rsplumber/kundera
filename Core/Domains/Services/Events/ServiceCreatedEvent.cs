namespace Core.Domains.Services.Events;

public sealed record ServiceCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_service_created";
}