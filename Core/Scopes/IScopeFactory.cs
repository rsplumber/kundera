using Core.Hashing;
using Core.Scopes.Exceptions;

namespace Core.Scopes;

public interface IScopeFactory
{
    Task<Scope> CreateAsync(string name, int sessionTokenExpireTimeInMinutes, int sessionRefreshTokenExpireTimeInMinutes, bool restricted = false);

    Task<Scope> CreateIdentityScopeAsync();
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

    public async Task<Scope> CreateAsync(string name, int sessionTokenExpireTimeInMinutes, int sessionRefreshTokenExpireTimeInMinutes, bool restricted = false)
    {
        var currentScope = await _scopeRepository.FindByNameAsync(name);
        if (currentScope is not null)
        {
            throw new ScopeAlreadyExistsException(name);
        }

        var scope = new Scope(name, _hashService)
        {
            SessionTokenExpireTimeInMinutes = sessionTokenExpireTimeInMinutes,
            SessionRefreshTokenExpireTimeInMinutes = sessionRefreshTokenExpireTimeInMinutes,
            Restricted = restricted
        };
        await _scopeRepository.AddAsync(scope);

        return scope;
    }

    public async Task<Scope> CreateIdentityScopeAsync()
    {
        var identityScope = await _scopeRepository.FindByNameAsync(EntityBaseValues.IdentityScopeName);

        if (identityScope is not null)
        {
            Console.WriteLine("------------ScopeSecret: " + identityScope.Secret);
            return identityScope;
        }

        var createdScope = await CreateAsync(EntityBaseValues.IdentityScopeName, 1000, 1000, true);
        Console.WriteLine("------------ScopeSecret: " + createdScope.Secret);
        return createdScope;
    }
}