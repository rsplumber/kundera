using Core.Domains.Credentials;

namespace Core.Domains.Sessions;

public interface ISessionRepository
{
    Task AddAsync(Session entity, CancellationToken cancellationToken = default);

    Task<Session?> FindAsync(Token id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Session entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);
}