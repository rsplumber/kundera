using Kite.Domain.Contracts;

namespace Auth.Core.Domains;

public interface ICredentialRepository : IRepository<Credential, UniqueIdentifier>, IDeleteService<UniqueIdentifier>, IUpdateService<Credential>
{
    ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}