namespace Core.Domains.Groups.Events;

public sealed record GroupRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_group_role_added";
}

public sealed record GroupRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_group_role_removed";
}