namespace Core.Domains.Auth.Credentials;

public interface ICredentialRepository
{
    Task AddAsync(Credential credential, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);

    Task DeleteAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default);
}