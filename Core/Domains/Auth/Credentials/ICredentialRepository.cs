using Core.Domains.Users;

namespace Core.Domains.Auth.Credentials;

public interface ICredentialRepository
{
    Task AddAsync(Credential credential, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(CredentialId id, CancellationToken cancellationToken = default);

    Task<List<Credential>> FindAsync(Username username, CancellationToken cancellationToken = default);

    Task DeleteAsync(CredentialId id, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Credential credential, CancellationToken cancellationToken = default);
}