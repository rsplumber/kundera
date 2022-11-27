using Core.Domains.Contracts;
using Core.Domains.Services.Events;
using Core.Domains.Services.Types;
using Core.Hashing;

namespace Core.Domains.Services;

public class Service : AggregateRoot
{
    protected Service()
    {
    }

    internal Service(Name name, IHashService hashService)
    {
        Name = name;
        Secret = ServiceSecret.From(hashService.Hash(Id.ToString(), Name.Value));
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    internal Service(Name name, ServiceSecret serviceSecret)
    {
        Name = name;
        Secret = serviceSecret;
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    public ServiceId Id { get; set; } = ServiceId.Generate();

    public Name Name { get; internal set; } = default!;

    public ServiceSecret Secret { get; internal set; } = default!;

    public ServiceStatus Status { get; internal set; } = default!;

    public void ChangeName(Name name) => Name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DiActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        Status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}