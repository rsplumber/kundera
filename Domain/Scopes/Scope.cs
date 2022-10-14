using Domain.Roles;
using Domain.Scopes.Events;
using Domain.Scopes.Exceptions;
using Domain.Scopes.Types;
using Domain.Services;
using Kite.Domain.Contracts;

namespace Domain.Scopes;

public class Scope : AggregateRoot<ScopeId>
{
    private ScopeStatus _status;
    private readonly List<ServiceId> _services = new();
    private readonly List<RoleId> _roles = new();

    protected Scope()
    {
    }

    private Scope(ScopeId id) : base(id)
    {
        ChangeStatus(ScopeStatus.Active);
        AddDomainEvent(new ScopeCreatedEvent(id));
    }

    public static async Task<Scope> FromAsync(Name name, IScopeRepository repository)
    {
        var id = ScopeId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        return new Scope(id);
    }

    public ScopeStatus Status => _status;

    public IReadOnlyCollection<ServiceId> Services => _services.AsReadOnly();

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

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