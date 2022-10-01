using Authentication.Domain.Types;
using Tes.Domain.Contracts;

namespace Authentication.Domain;

public interface ICredentialRepository : IRepository<UniqueIdentifier, Credential>, IDeleteService<UniqueIdentifier> , IUpdateService<Credential>
{
    ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}