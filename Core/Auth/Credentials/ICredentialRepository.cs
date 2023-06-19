namespace Core.Auth.Credentials;

public interface ICredentialRepository
{
    Task AddAsync(Credential credential, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Credential>> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Credential credential, CancellationToken cancellationToken = default);
}