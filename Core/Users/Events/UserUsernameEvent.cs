namespace Core.Users.Events;

public sealed record UserUsernameAddedEvent(Guid Id, string Username) : DomainEvent
{
    public override string Name => "kundera.users.username.added";
}

public sealed record UserUsernameRemovedEvent(Guid Id, string Username) : DomainEvent
{
    public override string Name => "kundera.users.username.removed";
}