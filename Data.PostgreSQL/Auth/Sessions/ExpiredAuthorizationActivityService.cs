using Core.Auth.Sessions;

namespace Data.Auth.Sessions;

internal sealed class ExpiredAuthorizationActivityService : IExpiredAuthorizationActivityService
{
    private readonly AppDbContext _dbContext;

    public ExpiredAuthorizationActivityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var query = from a in _dbContext.AuthorizationActivities
            where a.CreatedDateUtc.AddDays(10) >= DateTime.UtcNow
            group a by a.Session
            into g
            select g.OrderByDescending(x => x.CreatedDateUtc).Skip(5);
        _dbContext.AuthorizationActivities.RemoveRange(query.SelectMany(x => x));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}