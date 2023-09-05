using Core.Scopes;
using Microsoft.EntityFrameworkCore;

namespace Data.Scopes;

internal class ScopeRepository : IScopeRepository
{
    private readonly AppDbContext _dbContext;

    public ScopeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Scopes.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Scope?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Scopes
            .Include(scope => scope.Roles)
            .Include(scope => scope.Services)
            .FirstOrDefaultAsync(scope => scope.Id == id, cancellationToken);
    }

    public Task<Scope?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Scopes
            .Include(scope => scope.Roles)
            .Include(scope => scope.Services)
            .FirstOrDefaultAsync(scope => scope.Name == name, cancellationToken);
    }

    public Task<Scope?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default)
    {
        return _dbContext.Scopes
            .Include(scope => scope.Roles)
            .Include(scope => scope.Services)
            .FirstOrDefaultAsync(scope => scope.Secret == secret, cancellationToken);
    }

    public async Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Scopes.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentScope = await _dbContext.Scopes
            .FirstOrDefaultAsync(scope => scope.Id == id, cancellationToken);
        if (currentScope is null) return;
        _dbContext.Scopes.Remove(currentScope);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}