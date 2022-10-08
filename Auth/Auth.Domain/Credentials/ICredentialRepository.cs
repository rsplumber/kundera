using Tes.Domain.Contracts;

namespace Auth.Domain.Credentials;

public interface ICredentialRepository : IRepository<UniqueIdentifier, Credential>, IDeleteService<UniqueIdentifier> , IUpdateService<Credential>
{
    ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}