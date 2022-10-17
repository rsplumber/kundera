using Kite.Domain.Contracts;

namespace Managements.Domain.Users;

public interface IUserRepository : IRepository<User, UserId>, IUpdateService<User>, IDeleteService<UserId>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    ValueTask<User?> FindAsync(Username username, CancellationToken cancellationToken = default);
}