namespace Auth.Core.Entities;

public interface ICredentialRepository
{
    Task AddAsync(Credential entity, CancellationToken cancellationToken = default);

    Task<Credential?> FindAsync(UniqueIdentifier id, CancellationToken cancellationToken = default);

    Task DeleteAsync(UniqueIdentifier id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);

    Task DeleteExpiredAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default);
}