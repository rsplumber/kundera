using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagements.Domain.Tests.Scopes;

public class ScopeRepository : IScopeRepository
{
    private readonly List<Scope> _scopes;

    public ScopeRepository()
    {
        _scopes = new List<Scope>();
    }

    public async Task CreateAsync(Scope entity, CancellationToken cancellationToken = new CancellationToken())
    {
        _scopes.Add(entity);
    }

    public async Task<Scope?> FindAsync(ScopeId id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _scopes.FirstOrDefault(permission => permission.Id == id);
    }

    public async ValueTask<bool> ExistsAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        return _scopes.Exists(permission => permission.Id == id);
    }
}