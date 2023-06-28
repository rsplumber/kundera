using Core.Auth.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Credentials;

internal sealed class ExpiredCredentialsService : IExpiredCredentialsService
{
    private readonly AppDbContext _dbContext;

    public ExpiredCredentialsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var credentials = await _dbContext.Credentials
            .Where(model => model.ExpiresAtUtc != null && DateTime.UtcNow >= model.ExpiresAtUtc)
            .ToListAsync(cancellationToken);
        _dbContext.Credentials.RemoveRange(credentials);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}