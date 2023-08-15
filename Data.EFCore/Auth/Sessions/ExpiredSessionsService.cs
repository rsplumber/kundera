using Core.Auth.Sessions;

namespace Data.Auth.Sessions;

internal sealed class ExpiredSessionsService : IExpiredSessionsService
{
    private readonly AppDbContext _dbContext;

    public ExpiredSessionsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        _dbContext.Sessions.RemoveRange(_dbContext.Sessions
            .Where(session => session.RefreshTokenExpirationDateUtc >= DateTime.UtcNow));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}