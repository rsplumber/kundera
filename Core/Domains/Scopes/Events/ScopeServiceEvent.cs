using Core.Domains.Scopes.Types;
using Core.Domains.Services.Types;

namespace Core.Domains.Scopes.Events;

public sealed record ScopeServiceAddedEvent(ScopeId Id, ServiceId Service) : DomainEvent
{
    public override string Name => "scope_service_added";
}

public sealed record ScopeServiceRemovedEvent(ScopeId Id, ServiceId Service) : DomainEvent
{
    public override string Name => "scope_service_removed";
}