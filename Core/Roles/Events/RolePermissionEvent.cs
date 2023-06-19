namespace Core.Roles.Events;

public sealed record RolePermissionAddedEvent(Guid Id, Guid Permission) : DomainEvent
{
    public override string Name => "kundera.roles.permission.added";
}

public sealed record RolePermissionRemovedEvent(Guid Id, Guid Permission) : DomainEvent
{
    public override string Name => "kundera.roles.permission.removed";
}