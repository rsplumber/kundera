namespace Core.Domains.Scopes.Events;

public sealed record ScopeRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_scopes.role.added";
}

public sealed record ScopeRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_scopes.role.removed";
}