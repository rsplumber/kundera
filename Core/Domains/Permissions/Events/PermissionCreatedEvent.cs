namespace Core.Domains.Permissions.Events;

public sealed record PermissionCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_permissions.created";
}