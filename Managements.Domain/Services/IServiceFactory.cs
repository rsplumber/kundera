using Kite.Hashing;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;

namespace Managements.Domain.Services;

public interface IServiceFactory
{
    Task<Service> CreateAsync(Name name);

    Task<Service> CreateKunderaServiceAsync(ServiceSecret serviceSecret);
}

internal sealed class ServiceFactory : IServiceFactory
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IHashService _hashService;

    public ServiceFactory(IServiceRepository serviceRepository, IHashService hashService)
    {
        _serviceRepository = serviceRepository;
        _hashService = hashService;
    }

    public async Task<Service> CreateAsync(Name name)
    {
        var exists = await _serviceRepository.ExistsAsync(name);
        if (exists)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        return new Service(name, _hashService);
    }

    public async Task<Service> CreateKunderaServiceAsync(ServiceSecret serviceSecret)
    {
        var exists = await _serviceRepository.ExistsAsync(EntityBaseValues.KunderaServiceName);
        if (exists)
        {
            throw new ServiceAlreadyExistsException(EntityBaseValues.KunderaServiceName);
        }

        return new Service(EntityBaseValues.KunderaServiceName, serviceSecret);
    }
}