using Kite.Domain.Contracts;
using Kite.Hashing;
using Managements.Domain.Roles;
using Managements.Domain.Scopes.Events;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services;

namespace Managements.Domain.Scopes;

public class Scope : AggregateRoot<ScopeId>
{
    protected Scope()
    {
    }

    internal Scope(Name name, IHashService hashService) : base(ScopeId.Generate())
    {
        Name = name;
        Secret = ScopeSecret.From(hashService.Hash(Id.ToString(), Name.Value));
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(Id));
    }

    internal Scope(Name name, ScopeSecret scopeSecret) : base(ScopeId.Generate())
    {
        Name = name;
        Secret = scopeSecret;
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(Id));
    }

    public Name Name { get; internal set; }

    public ScopeSecret Secret { get; internal set; }

    public ScopeStatus Status { get; internal set; }

    public IReadOnlyCollection<ServiceId> Services { get; internal set; } = new List<ServiceId>();

    public IReadOnlyCollection<RoleId> Roles { get; internal set; } = new List<RoleId>();

    public void ChangeName(Name name) => Name = name;

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        Status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public void AddService(ServiceId service)
    {
        if (Has(service)) return;

        var modifiableServices = Services.ToList();
        modifiableServices.Add(service);
        Services = modifiableServices;
        AddDomainEvent(new ScopeServiceAddedEvent(Id, service));
    }

    public void RemoveService(ServiceId service)
    {
        if (!Has(service)) return;

        var modifiableServices = Services.ToList();
        modifiableServices.Remove(service);
        Services = modifiableServices;
        AddDomainEvent(new ScopeServiceRemovedEvent(Id, service));
    }

    public bool Has(ServiceId service)
    {
        return Services.Any(id => id == service);
    }


    public void AddRole(RoleId role)
    {
        if (Has(role)) return;

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Add(role);
        Roles = modifiableRoles;
        AddDomainEvent(new ScopeRoleAddedEvent(Id, role));
    }

    public void RemoveRole(RoleId role)
    {
        if (!Has(role)) return;

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Remove(role);
        Roles = modifiableRoles;
        AddDomainEvent(new ScopeRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return Roles.Any(id => id == role);
    }
}