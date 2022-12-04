using Core.Domains.Permissions.Types;

namespace Core.Domains.Permissions.Events;

public sealed record PermissionCreatedEvent(PermissionId Id) : DomainEvent
{
    public override string Name => "permission_created";
}