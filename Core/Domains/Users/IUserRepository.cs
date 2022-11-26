using Core.Domains.Users.Types;

namespace Core.Domains.Users;

public interface IUserRepository
{
    Task AddAsync(User entity, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(Username username, CancellationToken cancellationToken = default);

    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(UserId id, CancellationToken cancellationToken = default);
}