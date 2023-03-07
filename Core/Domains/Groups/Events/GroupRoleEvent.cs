namespace Core.Domains.Groups.Events;

public sealed record GroupRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_groups.role.added";
}

public sealed record GroupRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_groups.role.removed";
}