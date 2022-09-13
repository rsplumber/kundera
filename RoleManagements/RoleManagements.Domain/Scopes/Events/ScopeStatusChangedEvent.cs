using RoleManagements.Domain.Scopes.Types;

namespace RoleManagements.Domain.Scopes.Events;

public record ScopeStatusChangedEvent(ScopeId Id) : DomainEvent;