using System.Net;
using Authorization.Domain.Types;

namespace Authorization.Application;

public interface ISessionManagement
{
    Task SaveAsync(Certificate certificate, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default);

    Task UpdateAsync(Token token, DateTime lastUsageDate, IPAddress ipAddress, CancellationToken cancellationToken = default);

    Task DeleteAsync(Token token, CancellationToken cancellationToken = default);

    Task<SessionModel?> GetAsync(Token token, CancellationToken cancellationToken = default);

    Task<IEnumerable<SessionModel>> GetAllAsync(
        string? scope = null,
        Guid? userId = null,
        DateOnly? expireDate = null,
        DateOnly? lastUsageDateUtc = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);
}