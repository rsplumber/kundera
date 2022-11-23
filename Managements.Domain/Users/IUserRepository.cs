using Managements.Domain.Users.Types;

namespace Managements.Domain.Users;

public interface IUserRepository
{
    Task AddAsync(User entity, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(Username username, CancellationToken cancellationToken = default);

    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(UserId id, CancellationToken cancellationToken = default);
}