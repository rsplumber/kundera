using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Scopes;

namespace Core.Auth.Sessions;

public interface ISessionManagement
{
    Task<Certificate> SaveAsync(Credential credential, Scope scope, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);

    Task<Session?> GetAsync(string token, CancellationToken cancellationToken = default);
}