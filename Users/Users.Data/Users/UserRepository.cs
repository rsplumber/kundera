using Users.Domain.Users;

namespace Users.Data.Users;

internal class UserRepository : IUserRepository
{
    public async Task AddAsync(User entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}