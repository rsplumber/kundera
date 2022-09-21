using Authentication.Domain.Types;

namespace Authentication.Application;

public interface ICredentialRemoveService
{
    Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}