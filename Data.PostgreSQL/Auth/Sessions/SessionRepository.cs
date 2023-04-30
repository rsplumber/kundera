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
            .Include(session => session.Activity)
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
        var sessions = await _dbContext.Sessions
            .Where(model => DateTime.UtcNow >= model.ExpirationDateUtc.ToUniversalTime())
            .ToListAsync(cancellationToken);
        _dbContext.Sessions.RemoveRange(sessions);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}