using Core.Domains.Contracts;
using Core.Domains.Roles.Types;

namespace Core.Domains.Roles.Events;

[Event("role_created")]
public sealed record RoleCreatedEvent(RoleId Id) : DomainEvent;