using Kite.Domain.Contracts;
using Managements.Domain.Scopes.Types;

namespace Managements.Domain.Scopes;

public interface IScopeRepository : IRepository<Scope, ScopeId>, IUpdateService<Scope>, IDeleteService<ScopeId>
{
    Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(Name name, CancellationToken cancellationToken = default);

    Task<Scope?> FindAsync(ScopeSecret secret, CancellationToken cancellationToken = default);
}