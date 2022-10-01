using Authorization.Domain.Types;
using Tes.Domain.Contracts;

namespace Authorization.Domain;

public interface ISessionRepository : IRepository<Token, Session>, IUpdateService<Session>, IDeleteService<Token>
{
    ValueTask<bool> ExistsAsync(Token id, CancellationToken cancellationToken = default);
}