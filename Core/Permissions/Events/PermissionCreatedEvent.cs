namespace Core.Permissions.Events;

public sealed record PermissionCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.permissions.created";
}