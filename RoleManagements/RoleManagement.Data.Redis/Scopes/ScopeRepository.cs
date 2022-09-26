using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagement.Data.Redis.Scopes;

internal class ScopeRepository : IScopeRepository
{
    public async Task AddAsync(Scope entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Scope?> FindAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Scope entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}