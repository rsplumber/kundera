namespace Core.Groups.Events;

public sealed record GroupCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.groups.created";
}