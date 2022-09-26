using Redis.OM;
using Redis.OM.Searching;
using Users.Domain.Users;

namespace Users.Data.Redis.Users;

internal class UserRepository : IUserRepository
{
    private readonly RedisCollection<User> _users;

    public UserRepository(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<User>) provider.RedisCollection<User>();
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _users.InsertAsync(user);
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await _users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _users.UpdateAsync(user);
    }
}