using System.Net;
using Auth.Core.Domains;

namespace Auth.Core.Services;

public interface ISessionManagement
{
    ValueTask SaveAsync(Certificate certificate, Guid userId, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Token token, CancellationToken cancellationToken = default);

    ValueTask<Session?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Session>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
}