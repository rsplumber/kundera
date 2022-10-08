using Domain.Roles;
using Domain.Roles.Exceptions;
using Domain.Scopes.Events;
using Domain.Scopes.Exceptions;
using Domain.Scopes.Types;
using Domain.Services;
using Domain.Services.Exceptions;
using Domain.Services.Types;
using Tes.Domain.Contracts;

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

    public static async Task<Scope> CreateAsync(Name name, IScopeRepository repository)
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

    public void Activate() => ChangeStatus(ScopeStatus.Active);

    public void DeActivate() => ChangeStatus(ScopeStatus.DeActive);

    private void ChangeStatus(ScopeStatus status)
    {
        _status = status;
        AddDomainEvent(new ScopeStatusChangedEvent(Id));
    }

    public IReadOnlyCollection<ServiceId> Services => _services.AsReadOnly();

    public async Task AddServiceAsync(ServiceId service, IServiceRepository serviceRepository)
    {
        var serviceExist = await serviceRepository.ExistsAsync(service);
        if (!serviceExist)
        {
            throw new ServiceNotFoundException();
        }

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

    public async Task AddRoleAsync(RoleId role, IRoleRepository roleRepository)
    {
        var roleExist = await roleRepository.ExistsAsync(role);
        if (!roleExist)
        {
            throw new RoleNotFoundException();
        }

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