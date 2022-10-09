using Domain.Scopes.Types;
using Tes.Domain.Contracts;

namespace Domain.Scopes;

public interface IScopeRepository : IRepository<ScopeId, Scope>, IUpdateService<Scope>, IDeleteService<ScopeId>
{
    ValueTask<bool> ExistsAsync(ScopeId id, CancellationToken cancellationToken = default);
}