using Tes.Domain.Contracts;

namespace Auth.Domain.Sessions;

public interface ISessionRepository : IRepository<Token, Session>, IUpdateService<Session>, IDeleteService<Token>
{
    ValueTask<bool> ExistsAsync(Token id, CancellationToken cancellationToken = default);
}