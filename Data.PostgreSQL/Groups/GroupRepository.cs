using Core.Domains.Groups;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Data.PostgreSQL.Groups;

internal class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _dbContext;

    public GroupRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        await _dbContext.Groups.AddAsync(group, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Group?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Groups
            .Include(group => group.Roles)
            .Include(group => group.Parent)
            .Include(group => group.Children)
            .FirstOrDefaultAsync(credential => credential.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Group entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Groups.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Group?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Groups
            .Include(group => group.Roles)
            .Include(group => group.Parent)
            .Include(group => group.Children)
            .FirstOrDefaultAsync(group => group.Name == name, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentCredential = await _dbContext.Groups
            .FirstOrDefaultAsync(group => group.Id == id, cancellationToken);
        if(currentCredential is null) return;
        _dbContext.Groups.Remove(currentCredential);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}