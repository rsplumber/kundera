using Core.Domains.Auth.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Data.Auth.Sessions;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _dbContext;

    public SessionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Session session, CancellationToken cancellationToken = default)
    {
        await _dbContext.Sessions.AddAsync(session, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Session?> FindAsync(string token, CancellationToken cancellationToken = default)
    {
        return _dbContext.Sessions
            .Include(session => session.User)
            .Include(session => session.Credential)
            .Include(session => session.Scope)
            .FirstOrDefaultAsync(session => session.Id == token, cancellationToken);
    }

    public Task<Session?> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return _dbContext.Sessions.FirstOrDefaultAsync(sessions => sessions.RefreshToken == token, cancellationToken);
    }

    public Task<List<Session>> FindByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Sessions
            .Where(session => session.Credential.Id == credentialId)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var currentSession = await FindAsync(token, cancellationToken);
        if (currentSession is null) return;
        _dbContext.Sessions.Remove(currentSession);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var query = from a in _dbContext.Sessions
            where a.ExpirationDateUtc.AddDays(5) >= DateTime.UtcNow.ToUniversalTime()
            group a by a.Credential.Id
            into g
            select g.OrderByDescending(x => x.ExpirationDateUtc).Skip(2);
        _dbContext.Sessions.RemoveRange(query.SelectMany(x => x));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}