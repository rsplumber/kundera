using Core.Domains.Services.Events;
using Core.Hashing;

namespace Core.Domains.Services;

public class Service : BaseEntity
{
    protected Service()
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

    public string Name { get; internal set; } = default!;

    public string Secret { get; internal set; } = default!;

    public ServiceStatus Status { get; internal set; } = default!;

    public void ChangeName(string name) => Name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DeActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        Status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}