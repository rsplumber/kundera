using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

public sealed record UserCreatedEvent(UserId UserId) : DomainEvent
{
    public override string Name => "user_created";
}