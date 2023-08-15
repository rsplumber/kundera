using Core.Auth.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Credentials;

internal sealed class ExpiredAuthenticationActivityService : IExpiredAuthenticationActivityService
{
    private readonly AppDbContext _dbContext;

    public ExpiredAuthenticationActivityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var query = from a in _dbContext.AuthenticationActivities
            where a.CreatedDateUtc.AddDays(10) >= DateTime.UtcNow
            group a by a.Credential
            into g
            select g.OrderByDescending(x => x.CreatedDateUtc).Skip(5);
        var activities = await query.SelectMany(x => x).ToListAsync(cancellationToken);
        _dbContext.AuthenticationActivities.RemoveRange(activities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}