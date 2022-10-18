using Kite.Domain.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Scopes.Events;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services;

namespace Managements.Domain.Scopes;

public class Scope : AggregateRoot<ScopeId>
{
    private string _name;
    private ScopeStatus _status;
    private readonly List<ServiceId> _services = new();
    private readonly List<RoleId> _roles = new();

    protected Scope()
    {
    }

    private Scope(Name name) : base(ScopeId.Generate())
    {
        _name = name;
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(Id));
    }

    public static async Task<Scope> FromAsync(Name name, IScopeRepository repository)
    {
        var exists = await repository.ExistsAsync(name);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        return new Scope(name);
    }

    public string Name => _name;

    public ScopeStatus Status => _status;

    public IReadOnlyCollection<ServiceId> Services => _services.AsReadOnly();

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    public void ChangeName(Name name) => _name = name;

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        _status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public void AddService(ServiceId service)
    {
        if (Has(service)) return;

        _services.Add(service);
        AddDomainEvent(new ScopeServiceAddedEvent(Id, service));
    }

    public void RemoveService(ServiceId service)
    {
        if (!Has(service)) return;

        _services.Remove(service);
        AddDomainEvent(new ScopeServiceRemovedEvent(Id, service));
    }

    public bool Has(ServiceId service)
    {
        return _services.Any(id => id == service);
    }


    public void AddRole(RoleId role)
    {
        if (Has(role)) return;

        _roles.Add(role);
        AddDomainEvent(new ScopeRoleAddedEvent(Id, role));
    }

    public void RemoveRole(RoleId role)
    {
        if (!Has(role)) return;

        _roles.Remove(role);
        AddDomainEvent(new ScopeRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return _roles.Any(id => id == role);
    }
}