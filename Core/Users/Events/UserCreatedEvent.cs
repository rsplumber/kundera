namespace Core.Users.Events;

public sealed record UserCreatedEvent(Guid UserId) : DomainEvent
{
    public override string Name => "kundera.users.created";
}