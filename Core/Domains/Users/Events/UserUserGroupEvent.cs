namespace Core.Domains.Users.Events;

public sealed record UserJoinedGroupEvent(Guid Id, Guid Group) : DomainEvent
{
    public override string Name => "kundera.users.group.joined";
}

public sealed record UserRemovedGroupEvent(Guid Id, Guid Group) : DomainEvent
{
    public override string Name => "kundera.users.group.removed";
}