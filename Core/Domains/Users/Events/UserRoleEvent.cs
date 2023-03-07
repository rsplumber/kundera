namespace Core.Domains.Users.Events;

public sealed record UserRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_users.role.added";
}

public sealed record UserRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_users.role.removed";
}