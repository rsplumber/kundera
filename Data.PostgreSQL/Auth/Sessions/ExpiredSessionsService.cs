using Core.Auth.Sessions;
using Microsoft.EntityFrameworkCore;

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
        var query = from a in _dbContext.Sessions
            where a.ExpirationDateUtc.AddDays(5) >= DateTime.UtcNow.ToUniversalTime()
            group a by a.Credential.Id
            into g
            select g.OrderByDescending(x => x.ExpirationDateUtc).Skip(2);
        var sessions = await query.SelectMany(x => x).ToListAsync(cancellationToken);
        _dbContext.Sessions.RemoveRange(sessions);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}