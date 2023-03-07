namespace Core.Domains.Users.Events;

public sealed record UserCreatedEvent(Guid UserId) : DomainEvent
{
    public override string Name => "kundera_users.created";
}