using Auth.Domain.Credentials;

namespace Auth.Application;

public interface ICredentialRemoveService
{
    Task RemoveAsync(UniqueIdentifier uniqueIdentifier, CancellationToken cancellationToken = default);
}