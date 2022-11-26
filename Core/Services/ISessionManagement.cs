using Core.Domains.Scopes.Types;
using Core.Domains.Sessions;
using Core.Domains.Users.Types;

namespace Core.Services;

public interface ISessionManagement
{
    Task SaveAsync(Certificate certificate, UserId userId, ScopeId scopeId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token token, CancellationToken cancellationToken = default);

    Task<Session?> GetAsync(Token token, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default);
}