namespace Core.Domains.Users.Events;

public sealed record UserStatusChangedEvent(Guid UserId, UserStatus UserStatus) : DomainEvent
{
    public override string Name => "kundera_user_status";
}