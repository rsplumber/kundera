using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Data.Users;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .Include(user => user.Roles)
            .Include(user => user.Groups)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        if(currentUser is null) return;
        _dbContext.Users.Remove(currentUser);
        await _dbContext.SaveChangesAsync(cancellationToken);    }
}