using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Application;

public interface ICredentialService : ICredentialRemoveService
{
    Task CreateAsync(UniqueIdentifier uniqueIdentifier, UserId userId, Password password, CancellationToken cancellationToken = default);

    Task UpdateAsync(UniqueIdentifier uniqueIdentifier, Password newPassword, CancellationToken cancellationToken = default);
}