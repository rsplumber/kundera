using Managements.Domain.Contracts;
using Managements.Domain.Roles.Types;
using Managements.Domain.Scopes.Types;

namespace Managements.Domain.Scopes.Events;

[Event("scope_role_added")]
public sealed record ScopeRoleAddedEvent(ScopeId Id, RoleId Role) : DomainEvent;

[Event("scope_role_removed")]
public sealed record ScopeRoleRemovedEvent(ScopeId Id, RoleId Role) : DomainEvent;