namespace Core.Users.Events;

public sealed record UserStatusChangedEvent(Guid UserId, UserStatus UserStatus) : DomainEvent
{
    public override string Name => "kundera.users.status.changed";
}