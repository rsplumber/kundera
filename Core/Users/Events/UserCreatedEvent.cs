namespace Core.Users.Events;

public sealed record UserCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.users.created";
}