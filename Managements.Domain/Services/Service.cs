using Kite.Domain.Contracts;
using Kite.Hashing;
using Managements.Domain.Services.Events;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public class Service : AggregateRoot<ServiceId>
{
    private string _name;
    private string _secret;
    private ServiceStatus _status;

    protected Service()
    {
    }

    private Service(Name name, IHashService hashService) : base(ServiceId.Generate())
    {
        _name = name;
        _secret = hashService.Hash(Id.ToString());
        ChangeStatus(ServiceStatus.Active);
        AddDomainEvent(new ServiceCreatedEvent(Id));
    }

    public static async Task<Service> FromAsync(Name name, IHashService hashService, IServiceRepository repository)
    {
        var exists = await repository.ExistsAsync(name);
        if (exists)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        return new Service(name, hashService);
    }


    public Name Name => _name;

    public ServiceSecret Secret => ServiceSecret.From(_secret);

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