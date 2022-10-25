using Kite.Hashing;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;

namespace Managements.Domain.Scopes;

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

        return new Scope(name, _hashService);
    }

    public async Task<Scope> CreateIdentityScopeAsync(ScopeSecret serviceSecret)
    {
        var exists = await _scopeRepository.ExistsAsync(EntityBaseValues.IdentityScopeName);
        if (exists)
        {
            throw new ScopeAlreadyExistsException(EntityBaseValues.IdentityScopeName);
        }

        return new Scope(EntityBaseValues.IdentityScopeName, serviceSecret);
    }
}