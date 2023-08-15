using Core.Auth.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Credentials;

internal class CredentialRepository : ICredentialRepository
{
    private readonly AppDbContext _dbContext;

    public CredentialRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Credential credential, CancellationToken cancellationToken = default)
    {
        await _dbContext.Credentials.AddAsync(credential, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Credential?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Credentials
            .Include(credential => credential.User)
            .FirstOrDefaultAsync(credential => credential.Id == id, cancellationToken);
    }

    public Task<List<Credential>> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _dbContext.Credentials
            .Include(credential => credential.User)
            .Where(credential => credential.Username == username)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var currentCredential = await _dbContext.Credentials
            .FirstOrDefaultAsync(credential => credential.Id == id, cancellationToken);
        if (currentCredential is null) return;
        _dbContext.Credentials.Remove(currentCredential);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Credentials.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}