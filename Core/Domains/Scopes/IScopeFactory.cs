using Core.Domains.Scopes.Exceptions;
using Core.Hashing;

namespace Core.Domains.Scopes;

public interface IScopeFactory
{
    Task<Scope> CreateAsync(string name);

    Task<Scope> CreateIdentityScopeAsync(string scopeSecret);
}

public sealed class ScopeFactory : IScopeFactory
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IHashService _hashService;


    public ScopeFactory(IScopeRepository scopeRepository, IHashService hashService)
    {
        _scopeRepository = scopeRepository;
        _hashService = hashService;
    }

    async Task<Scope> IScopeFactory.CreateAsync(string name)
    {
        var currentScope = await _scopeRepository.FindByNameAsync(name);
        if (currentScope is not null)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        var scope = new Scope(name, _hashService);
        await _scopeRepository.AddAsync(scope);

        return scope;
    }

    public async Task<Scope> CreateIdentityScopeAsync(string scopeSecret)
    {
        var identityScope = await _scopeRepository.FindBySecretAsync(EntityBaseValues.IdentityScopeName);
        if (identityScope is not null)
        {
            throw new ScopeAlreadyExistsException(EntityBaseValues.IdentityScopeName);
        }

        var scope = new Scope(EntityBaseValues.IdentityScopeName, scopeSecret);
        await _scopeRepository.AddAsync(scope);

        return scope;
    }
}