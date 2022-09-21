using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class CredentialRepository : ICredentialRepository
{
    public async Task AddAsync(Credential entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<Credential?> FindAsync(UniqueIdentifier id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(UniqueIdentifier id, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> ExistsAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}