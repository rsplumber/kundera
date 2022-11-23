using Managements.Domain.Contracts;
using Managements.Domain.Roles.Types;

namespace Managements.Domain.Roles.Events;

[Event("role_created")]
public sealed record RoleCreatedEvent(RoleId Id) : DomainEvent;