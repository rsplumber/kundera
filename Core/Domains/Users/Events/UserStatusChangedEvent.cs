using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

public sealed record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent
{
    public override string Name => "user_status";
}