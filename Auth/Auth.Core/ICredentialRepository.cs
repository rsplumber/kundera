using Kite.Domain.Contracts;

namespace Auth.Core;

public interface ICredentialRepository : IRepository<Credential, UniqueIdentifier>, IDeleteService<UniqueIdentifier>, IUpdateService<Credential>
{
    Task<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}