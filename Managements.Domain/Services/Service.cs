using Kite.Domain.Contracts;
using Managements.Domain.Services.Events;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public class Service : AggregateRoot<ServiceId>
{
    private string _name;
    private ServiceStatus _status;

    protected Service()
    {
    }

    private Service(Name name) : base(ServiceId.Generate())
    {
        _name = name;
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    public static async Task<Service> FromAsync(Name name, IServiceRepository repository)
    {
        var exists = await repository.ExistsAsync(name);
        if (exists)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        return new Service(name);
    }


    public string Name => _name;

    public ServiceStatus Status => _status;

    public void ChangeName(Name name) => _name = name;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DiActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        _status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}