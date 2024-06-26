﻿using Core.Hashing;
using Core.Services.Exceptions;

namespace Core.Services;

public interface IServiceFactory
{
    Task<Service> CreateAsync(string name);

    Task<Service> CreateKunderaServiceAsync(string serviceSecret);
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

    public async Task<Service> CreateAsync(string name)
    {
        var currentService = await _serviceRepository.FindByNameAsync(name);
        if (currentService is not null)
        {
            throw new ServiceAlreadyExistsException(name);
        }

        var service = new Service(name, _hashService);
        await _serviceRepository.AddAsync(service);
        return service;
    }

    public async Task<Service> CreateKunderaServiceAsync(string serviceSecret)
    {
        var kunderaService = await _serviceRepository.FindBySecretAsync(EntityBaseValues.KunderaServiceName);
        if (kunderaService is not null)
        {
            Console.WriteLine("------------ServiceSecret: " + kunderaService.Secret);
            return kunderaService;
        }

        var service = new Service(EntityBaseValues.KunderaServiceName, serviceSecret);
        await _serviceRepository.AddAsync(service);
        Console.WriteLine("------------ServiceSecret: " + service.Secret);
        return service;
    }
}