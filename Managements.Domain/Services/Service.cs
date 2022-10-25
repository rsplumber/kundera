using Kite.Domain.Contracts;
using Kite.Hashing;
using Managements.Domain.Services.Events;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public class Service : AggregateRoot<ServiceId>
{
    protected Service()
    {
    }

    internal Service(Name name, IHashService hashService) : base(ServiceId.Generate())
    {
        Name = name;
        Secret = ServiceSecret.From(hashService.Hash(Id.ToString(), Name.Value));
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    internal Service(Name name, ServiceSecret serviceSecret) : base(ServiceId.Generate())
    {
        Name = name;
        Secret = serviceSecret;
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    public Name Name { get; internal set; }

    public ServiceSecret Secret { get; internal set; }

    public ServiceStatus Status { get; internal set; }

    public void ChangeName(Name name) => Name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DiActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        Status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}