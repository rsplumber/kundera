using Core.Hashing;
using Core.Permissions;
using Core.Services.Events;

namespace Core.Services;

public class Service : BaseEntity
{
    public Service()
    {
    }

    internal Service(string name, IHashService hashService)
    {
        Name = name;
        var hashKey = Random.Shared.RandomCharsAndNumbers(6);
        Secret = hashService.HashAsync(hashKey, Id.ToString(), Name).Result;
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

    public void ChangeName(string name) => Name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DeActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        Status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}