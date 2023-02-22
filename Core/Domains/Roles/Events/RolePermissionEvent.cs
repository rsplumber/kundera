namespace Core.Domains.Roles.Events;

public sealed record RolePermissionAddedEvent(Guid Id, Guid Permission) : DomainEvent
{
    public override string Name => "kundera_role_permission_added";
}

public sealed record RolePermissionRemovedEvent(Guid Id, Guid Permission) : DomainEvent
{
    public override string Name => "kundera_role_permission_removed";
}