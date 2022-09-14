using Authorization.Domain.Types;
using Tes.Domain.Contracts;

namespace Authorization.Domain;

public interface ISessionRepository : IRepository<Token, Session>, IUpdateService<Session>, IDeleteService<Token>
{
    Task<IEnumerable<Session>> GetAllAsync(
        string? scope = null,
        Guid? userId = null,
        DateOnly? expireDate = null,
        DateOnly? lastUsageDateUtc = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);
}