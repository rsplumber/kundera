namespace Core.Domains.Auth.Sessions;

public interface ISessionRepository
{
    Task AddAsync(Session entity, CancellationToken cancellationToken = default);

    Task<Session?> FindAsync(string token, CancellationToken cancellationToken = default);

    Task<Session?> FindByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Session entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);
}