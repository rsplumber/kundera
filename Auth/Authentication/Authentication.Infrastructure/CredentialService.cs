using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class CredentialService : ICredentialService
{
    public async Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UniqueIdentifier uniqueIdentifier, Password newPassword, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}