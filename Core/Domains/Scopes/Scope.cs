using Core.Domains.Roles;
using Core.Domains.Scopes.Events;
using Core.Domains.Services;
using Core.Hashing;

namespace Core.Domains.Scopes;

public class Scope : BaseEntity
{
    protected Scope()
    {
    }

    internal Scope(string name, IHashService hashService)
    {
        Name = name;
        Secret = hashService.Hash(Id.ToString(), Name);
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(Id));
    }

    internal Scope(string name, string scopeSecret)
    {
        Name = name;
        Secret = scopeSecret;
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(Id));
    }

    public Guid Id { get; internal set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public string Secret { get; internal set; } = default!;

    public ScopeStatus Status { get; internal set; } = default!;

    public HashSet<Service> Services { get; internal set; } = new();

    public HashSet<Role> Roles { get; internal set; } = new();

    public void ChangeName(string name) => Name = name;

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        Status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public void Add(Service service)
    {
        if (Has(service)) return;
        Services.Add(service);
        AddDomainEvent(new ScopeServiceAddedEvent(Id, service.Id));
    }

    public void Remove(Service service)
    {
        if (!Has(service)) return;
        Services.Remove(service);
        AddDomainEvent(new ScopeServiceRemovedEvent(Id, service.Id));
    }

    public bool Has(Service service)=> Services.Any(s => s == service);


    public void Add(Role role)
    {
        if (Has(role)) return;
        Roles.Add(role);
        AddDomainEvent(new ScopeRoleAddedEvent(Id, role.Id));
    }

    public void Remove(Role role)
    {
        if (!Has(role)) return;
        Roles.Remove(role);
        AddDomainEvent(new ScopeRoleRemovedEvent(Id, role.Id));
    }

    public bool Has(Role role) =>       Roles.Any(r => r == role);
}