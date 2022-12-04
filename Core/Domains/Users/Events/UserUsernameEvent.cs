using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

public sealed record UserUsernameAddedEvent(UserId Id, Username Username) : DomainEvent
{
    public override string Name => "user_username_added";
}

public sealed record UserUsernameRemovedEvent(UserId Id, Username Username) : DomainEvent
{
    public override string Name => "user_username_removed";
}