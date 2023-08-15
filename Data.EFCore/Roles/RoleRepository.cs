using Core.Roles;
using Microsoft.EntityFrameworkCore;

namespace Data.Roles;

internal class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Roles.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Role?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles
            .Include(role => role.Permissions)
            .FirstOrDefaultAsync(role => role.Id == id, cancellationToken);
    }

    public Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles
            .Include(role => role.Permissions)
            .FirstOrDefaultAsync(role => role.Name == name, cancellationToken);
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Roles.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentRole = await _dbContext.Roles
            .FirstOrDefaultAsync(role => role.Id == id, cancellationToken);
        if(currentRole is null) return;
        _dbContext.Roles.Remove(currentRole);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}