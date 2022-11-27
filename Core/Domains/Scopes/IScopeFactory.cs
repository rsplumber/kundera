﻿using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using Core.Hashing;

namespace Core.Domains.Scopes;

public interface IScopeFactory
{
    Task<Scope> CreateAsync(Name name);

    Task<Scope> CreateIdentityScopeAsync(ScopeSecret serviceSecret);
}

internal sealed class ScopeFactory : IScopeFactory
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IHashService _hashService;


    public ScopeFactory(IScopeRepository scopeRepository, IHashService hashService)
    {
        _scopeRepository = scopeRepository;
        _hashService = hashService;
    }

    async Task<Scope> IScopeFactory.CreateAsync(Name name)
    {
        var exists = await _scopeRepository.ExistsAsync(name);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        var scope = new Scope(name, _hashService);
        await _scopeRepository.AddAsync(scope);

        return scope;
    }

    public async Task<Scope> CreateIdentityScopeAsync(ScopeSecret serviceSecret)
    {
        var exists = await _scopeRepository.ExistsAsync(EntityBaseValues.IdentityScopeName);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(EntityBaseValues.IdentityScopeName);
        }

        var scope = new Scope(EntityBaseValues.IdentityScopeName, serviceSecret);
        await _scopeRepository.AddAsync(scope);

        return scope;
    }
}