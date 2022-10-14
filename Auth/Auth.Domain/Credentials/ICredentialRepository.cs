using Kite.Domain.Contracts;

namespace Auth.Domain.Credentials;

public interface ICredentialRepository : IRepository<Credential, UniqueIdentifier>, IDeleteService<UniqueIdentifier>, IUpdateService<Credential>
{
    ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}