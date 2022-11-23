using Managements.Domain.Contracts;
using Managements.Domain.Permissions.Types;

namespace Managements.Domain.Permissions.Events;

[Event("permission_created")]
public sealed record PermissionCreatedEvent(PermissionId Id) : DomainEvent;