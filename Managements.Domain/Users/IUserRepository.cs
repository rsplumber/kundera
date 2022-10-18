using Kite.Domain.Contracts;

namespace Managements.Domain.Users;

public interface IUserRepository : IRepository<User, UserId>, IUpdateService<User>, IDeleteService<UserId>
{
    Task<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(Username username, CancellationToken cancellationToken = default);
}