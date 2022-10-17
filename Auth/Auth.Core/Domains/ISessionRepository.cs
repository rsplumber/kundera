using Kite.Domain.Contracts;

namespace Auth.Core.Domains;

public interface ISessionRepository : IRepository<Session, Token>, IUpdateService<Session>, IDeleteService<Token>
{
    ValueTask<bool> ExistsAsync(Token id, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Session>> FindAsync(Guid userId, CancellationToken cancellationToken = default);
}