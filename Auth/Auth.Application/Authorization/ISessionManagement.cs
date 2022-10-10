using System.Net;
using Auth.Domain.Sessions;

namespace Auth.Application.Authorization;

public interface ISessionManagement
{
    ValueTask SaveAsync(Certificate certificate, Guid userId, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(Token token, CancellationToken cancellationToken = default);

    ValueTask<SessionModel?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<SessionModel>> GetAllAsync(CancellationToken cancellationToken = default);
}