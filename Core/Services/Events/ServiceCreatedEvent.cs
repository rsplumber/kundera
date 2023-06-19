namespace Core.Services.Events;

public sealed record ServiceCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.services.created";
}