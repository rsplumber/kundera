namespace Core.Domains.Scopes.Events;

public sealed record ScopeRoleAddedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_scope_role_added";
}

public sealed record ScopeRoleRemovedEvent(Guid Id, Guid Role) : DomainEvent
{
    public override string Name => "kundera_scope_role_removed";
}