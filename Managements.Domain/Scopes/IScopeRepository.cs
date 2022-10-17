using Kite.Domain.Contracts;

namespace Managements.Domain.Scopes;

public interface IScopeRepository : IRepository<Scope, ScopeId>, IUpdateService<Scope>, IDeleteService<ScopeId>
{
    ValueTask<bool> ExistsAsync(ScopeId id, CancellationToken cancellationToken = default);
}