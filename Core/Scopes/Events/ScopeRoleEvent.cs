namespace Core.Scopes.Events;

public sealed record ScopeRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera.scopes.role.added";
}

public sealed record ScopeRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera.scopes.role.removed";
}