using Core.Domains.Auth.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Sessions;

internal sealed class AuthorizationActivityRepository : IAuthorizationActivityRepository
{
    private readonly AppDbContext _dbContext;

    public AuthorizationActivityRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AuthorizationActivity authenticationActivity, CancellationToken cancellationToken = default)
    {
        await _dbContext.AuthorizationActivities.AddAsync(authenticationActivity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<AuthorizationActivity?> FindLastBySessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return _dbContext.AuthorizationActivities.LastOrDefaultAsync(activity => activity.Session == sessionId, cancellationToken);
    }

    public Task<List<AuthorizationActivity>> FindBySessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return _dbContext.AuthorizationActivities
            .Where(activity => activity.Session == sessionId)
            .ToListAsync(cancellationToken);
    }
}