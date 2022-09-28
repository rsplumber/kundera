using Tes.Domain.Contracts;

namespace Users.Domain.Users;

public interface IUserRepository : IRepository<UserId, User>, IUpdateService<User>
{
    ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User> FindAsync(Username username, CancellationToken cancellationToken = default);
}