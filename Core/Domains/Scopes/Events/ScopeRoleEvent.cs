using Core.Domains.Roles.Types;
using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

public sealed record ScopeRoleAddedEvent(ScopeId Id, RoleId Role) : DomainEvent
{
    public override string Name => "scope_role_added";
}

public sealed record ScopeRoleRemovedEvent(ScopeId Id, RoleId Role) : DomainEvent
{
    public override string Name => "scope_role_removed";
}