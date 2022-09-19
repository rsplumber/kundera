using Users.Domain.Users;
using Users.Domain.Users.Types;

namespace Users.Data.Users;

internal class UserRepository : IUserRepository
{
    public Task CreateAsync(User entity, CancellationToken cancellationToken = default)
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
}