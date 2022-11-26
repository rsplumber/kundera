using Core.Domains.Contracts;
using Core.Domains.Permissions.Types;

namespace Core.Domains.Permissions.Events;

[Event("permission_created")]
public sealed record PermissionCreatedEvent(PermissionId Id) : DomainEvent;