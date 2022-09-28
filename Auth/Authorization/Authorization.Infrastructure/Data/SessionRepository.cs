using Authorization.Domain;
using Authorization.Domain.Types;

namespace Authorization.Infrastructure.Data;

internal sealed class SessionRepository : ISessionRepository
{
    public async Task AddAsync(Session entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Session?> FindAsync(Token id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Session entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Token id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Session>> GetAllAsync(string? scope = null, Guid? userId = null, DateOnly? expireDate = null, DateOnly? lastUsageDateUtc = null, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}