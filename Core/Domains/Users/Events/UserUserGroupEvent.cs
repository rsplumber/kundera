namespace Core.Domains.Users.Events;

public sealed record UserJoinedGroupEvent(Guid Id, Guid Group) : DomainEvent
{
    public override string Name => "kundera_user_joined_group";
}

public sealed record UserRemovedGroupEvent(Guid Id, Guid Group) : DomainEvent
{
    public override string Name => "kundera_user_removed_group";
}