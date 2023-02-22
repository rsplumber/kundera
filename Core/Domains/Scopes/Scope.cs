using Core.Domains.Scopes.Events;
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

    public HashSet<Guid> Services { get; internal set; } = new();

    public HashSet<Guid> Roles { get; internal set; } = new();

    public void ChangeName(string name) => Name = name;

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        Status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public void AddService(Guid service)
    {
        if (HasService(service)) return;
        Services.Add(service);
        AddDomainEvent(new ScopeServiceAddedEvent(Id, service));
    }

    public void RemoveService(Guid service)
    {
        if (!HasService(service)) return;
        Services.Remove(service);
        AddDomainEvent(new ScopeServiceRemovedEvent(Id, service));
    }

    public bool HasService(Guid service)
    {
        return Services.Any(id => id == service);
    }


    public void AddRole(Guid role)
    {
        if (HasRole(role)) return;
        Roles.Add(role);
        AddDomainEvent(new ScopeRoleAddedEvent(Id, role));
    }

    public void RemoveRole(Guid role)
    {
        if (!HasRole(role)) return;
        Roles.Remove(role);
        AddDomainEvent(new ScopeRoleRemovedEvent(Id, role));
    }

    public bool HasRole(Guid role)
    {
        return Roles.Any(id => id == role);
    }
}