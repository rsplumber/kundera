using Core.Domains.Auth.Credentials;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Credentials;

internal sealed class AuthenticationActivityRepository : IAuthenticationActivityRepository
{
    private readonly AppDbContext _dbContext;

    public AuthenticationActivityRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AuthenticationActivity authenticationActivity, CancellationToken cancellationToken = default)
    {
        await _dbContext.AuthenticationActivities.AddAsync(authenticationActivity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<AuthenticationActivity?> FindLastByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default)
    {
        return _dbContext.AuthenticationActivities.LastOrDefaultAsync(activity => activity.Credential == credentialId, cancellationToken);
    }

    public Task<List<AuthenticationActivity>> FindCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default)
    {
        return _dbContext.AuthenticationActivities
            .Where(activity => activity.Credential == credentialId)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveExpiredActivitiesAsync(CancellationToken cancellationToken = default)
    {
        var query = from a in _dbContext.AuthenticationActivities
            where a.CreatedDateUtc.AddDays(10) >= DateTime.UtcNow.ToUniversalTime()
            group a by a.Credential
            into g
            select g.OrderByDescending(x => x.CreatedDateUtc).Skip(5);
        _dbContext.AuthenticationActivities.RemoveRange(query.SelectMany(x => x));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}