namespace Core.Roles.Events;

public sealed record RoleCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.roles.created";
}