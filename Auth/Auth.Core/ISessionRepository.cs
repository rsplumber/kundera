using Kite.Domain.Contracts;

namespace Auth.Core;

public interface ISessionRepository : IRepository<Session, Token>, IUpdateService<Session>, IDeleteService<Token>
{
    Task<bool> ExistsAsync(Token id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Session>> FindAsync(Guid userId, CancellationToken cancellationToken = default);
}