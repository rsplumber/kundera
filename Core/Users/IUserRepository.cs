namespace Core.Users;

public interface IUserRepository
{
    Task AddAsync(User entity, CancellationToken cancellationToken = default);

    Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task UpdateAsync(User entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}