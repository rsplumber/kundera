using RoleManagements.Domain.Scopes.Types;

namespace RoleManagements.Domain.Scopes.Events;

public record ScopeCreatedEvent(ScopeId Id) : DomainEvent;