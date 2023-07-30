namespace Core.Auth.Sessions;

public interface ISessionRepository
{
    Task AddAsync(Session entity, CancellationToken cancellationToken = default);

    Task<Session?> FindAsync(string token, CancellationToken cancellationToken = default);

    Task<List<Session>> FindByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);
}