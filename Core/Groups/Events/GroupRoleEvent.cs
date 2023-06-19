namespace Core.Groups.Events;

public sealed record GroupRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera.groups.role.added";
}

public sealed record GroupRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera.groups.role.removed";
}