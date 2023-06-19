namespace Core.Scopes;

public interface IScopeRepository
{
    Task AddAsync(Scope entity, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Scope?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Scope?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default);

    Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}