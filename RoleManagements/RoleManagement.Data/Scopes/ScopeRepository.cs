using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagement.Data.Scopes;

internal class ScopeRepository : IScopeRepository
{
    public async Task CreateAsync(Scope entity, CancellationToken cancellationToken = default)
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
}