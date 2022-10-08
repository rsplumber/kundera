using Domain.Roles;
using Domain.Services;

namespace Domain.Scopes.Events;

public record ScopeRoleAddedEvent(ScopeId Id, RoleId Role) : DomainEvent;

public record ScopeRoleRemovedEvent(ScopeId Id, RoleId Role) : DomainEvent;