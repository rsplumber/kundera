using Users.Domain.Users;
using Users.Domain.Users.Types;

namespace Users.Domain.Tests.Services;

public class UserRepository : IUserRepository
{
    private static readonly List<User> Users = new();


    public Task CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        Users.Add(entity);
        return Task.CompletedTask;
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(Users.Exists(u => u.Username == username));
    }
}