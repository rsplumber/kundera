using Core.Domains.Roles.Types;

namespace Core.Domains.Roles.Events;

public sealed record RoleCreatedEvent(RoleId Id) : DomainEvent
{
    public override string Name => "role_created";
}