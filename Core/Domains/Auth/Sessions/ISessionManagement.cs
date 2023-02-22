using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Scopes;

namespace Core.Domains.Auth.Sessions;

public interface ISessionManagement
{
    Task<Certificate> SaveAsync(Credential credential, Scope scope, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);

    Task<Session?> GetAsync(string token, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}