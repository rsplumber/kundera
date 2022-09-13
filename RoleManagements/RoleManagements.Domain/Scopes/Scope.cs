using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Scopes.Events;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Scopes;

public class Scope : AggregateRoot<ScopeId>
{
    private readonly string _title;
    private ScopeStatus _status;
    private readonly List<ServiceId> _services;
    private readonly List<RoleId> _roles;

    protected Scope()
    {
    }

    private Scope(ScopeId id, Name title) : base(id)
    {
        _services = new List<ServiceId>();
        _roles = new List<RoleId>();
        _title = title;
        _status = ScopeStatus.Active;
        AddDomainEvent(new ScopeCreatedEvent(id));
    }

    public static async Task<Scope> CreateAsync(Name name, Name title, IScopeRepository repository)
    {
        var id = ScopeId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        return new Scope(id, title);
    }

    public string Title => _title;

    public ScopeStatus Status => _status;

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        _status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public IReadOnlyCollection<ServiceId> Services => _services.AsReadOnly();

    public void AddService(ServiceId service)
    {
        if (HasService(service)) return;
        _services.Add(service);
    }

    public void RemoveService(ServiceId service)
    {
        if (!HasService(service)) return;
        _services.Remove(service);
    }

    public bool HasService(ServiceId service)
    {
        return _services.Exists(id => id == service);
    }

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    public void AddRole(RoleId role)
    {
        if (HasRole(role)) return;
        _roles.Add(role);
    }

    public void RemoveRole(RoleId role)
    {
        if (!HasRole(role)) return;
        _roles.Remove(role);
    }

    public bool HasRole(RoleId role)
    {
        return _roles.Exists(id => id == role);
    }
}