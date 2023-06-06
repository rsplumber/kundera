using Core.Domains.Permissions;
using Core.Domains.Services.Events;
using Core.Hashing;

namespace Core.Domains.Services;

public class Service : BaseEntity
{
    public Service()
    {
    }

    internal Service(string name, IHashService hashService)
    {
        Name = name;
        Secret = hashService.Hash(Id.ToString(), Name);
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    internal Service(string name, string serviceSecret)
    {
        Name = name;
        Secret = serviceSecret;
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public string Secret { get; set; } = default!;

    public ServiceStatus Status { get; set; }

    public List<Permission> Permissions { get; set; } = new();

    public void AddPermission(string name, IDictionary<string, string>? meta = null)
    {
        var finalName = $"{Name}_{name}";
        if (Permissions.Any(p => p.Name == finalName)) return;
        Permissions.Add(new Permission(finalName, meta));
    }

    public void RemovePermission(string name)
    {
        var finalName = $"{Name}_{name}";
        var selectedPermission = Permissions.FirstOrDefault(p => p.Name == finalName);
        if (selectedPermission is null) return;
        Permissions.Remove(selectedPermission);
    }

    public void RemovePermission(Guid permissionId)
    {
        var selectedPermission = Permissions.FirstOrDefault(p => p.Id == permissionId);
        if (selectedPermission is null) return;
        Permissions.Remove(selectedPermission);
    }

    public void ChangeName(string name) => Name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DeActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        Status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}