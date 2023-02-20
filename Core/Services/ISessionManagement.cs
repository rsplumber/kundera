using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Core.Domains.Users.Types;

namespace Core.Services;

public interface ISessionManagement
{
    Task<Certificate> SaveAsync(Credential credential, Scope scope, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token token, CancellationToken cancellationToken = default);

    Task<Session?> GetAsync(Token token, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default);
}