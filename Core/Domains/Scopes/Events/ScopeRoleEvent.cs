using Core.Domains.Contracts;
using Core.Domains.Roles.Types;
using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes.Events;

[Event("scope_role_added")]
public sealed record ScopeRoleAddedEvent(ScopeId Id, RoleId Role) : DomainEvent;

[Event("scope_role_removed")]
public sealed record ScopeRoleRemovedEvent(ScopeId Id, RoleId Role) : DomainEvent;