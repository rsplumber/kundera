using Core.Domains.Scopes.Types;

namespace Core.Domains.Scopes;

public interface IScopeRepository
{
    Task AddAsync(Scope entity, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(ScopeId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(ScopeSecret secret, CancellationToken cancellationToken = default);

    Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(ScopeId id, CancellationToken cancellationToken = default);
}