using RoleManagements.Domain.Services.Events;
using RoleManagements.Domain.Services.Exceptions;
using RoleManagements.Domain.Services.Types;
using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Services;

public class Service : AggregateRoot<ServiceId>
{
    private ServiceStatus _status;

    protected Service()
    {
    }

    private Service(ServiceId id) : base(id)
    {
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(id));
    }

    public static async Task<Service> CreateAsync(Name name, IServiceRepository repository)
    {
        var id = ServiceId.From(name);
        var exists = await repository.ExistsAsync(id);
        if (exists)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        return new Service(id);
    }

    public ServiceStatus Status => _status;

    public void Activate() => ChangeStatus(ServiceStatus.Active);

    public void DiActivate() => ChangeStatus(ServiceStatus.DeActive);

    private void ChangeStatus(ServiceStatus status)
    {
        _status = status;
        AddDomainEvent(new ServiceStatusChangedEvent(Id));
    }
}