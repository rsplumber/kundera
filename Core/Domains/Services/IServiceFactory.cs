﻿using Core.Domains.Services.Exceptions;
using Core.Domains.Services.Types;
using Core.Hashing;

namespace Core.Domains.Services;

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
        var currentService = await _serviceRepository.FindAsync(name);
        if (currentService is not null)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        var service = new Service(name, _hashService);
        await _serviceRepository.AddAsync(service);
        return service;
    }

    public async Task<Service> CreateKunderaServiceAsync(ServiceSecret serviceSecret)
    {
        var kunderaService = await _serviceRepository.FindAsync(EntityBaseValues.KunderaServiceName);
        if (kunderaService is not null)
        {
            throw new ServiceAlreadyExistsException(EntityBaseValues.KunderaServiceName);
        }

        var service = new Service(EntityBaseValues.KunderaServiceName, serviceSecret);
        await _serviceRepository.AddAsync(service);
        return service;
    }
}