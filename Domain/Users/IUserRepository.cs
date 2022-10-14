using Kite.Domain.Contracts;

namespace Domain.Users;

public interface IUserRepository : IRepository<User, UserId>, IUpdateService<User>, IDeleteService<UserId>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User> FindAsync(Username username, CancellationToken cancellationToken = default);
}