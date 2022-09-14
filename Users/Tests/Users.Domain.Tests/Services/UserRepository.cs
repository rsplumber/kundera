using Users.Domain.Users;
using Users.Domain.Users.Types;

namespace Users.Domain.Tests.Services;

public class UserRepository : IUserRepository
{
    private static List<User>? _users = new List<User>();
    

    public Task CreateAsync(User entity, CancellationToken cancellationToken = new CancellationToken())
    {
        _users.Add(entity);
        return Task.CompletedTask;
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = new CancellationToken())
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_users.Exists(u => u.Username == username));
    }

    public ValueTask<bool> ExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_users.Exists(u => u.PhoneNumber == phoneNumber));
    }

    public ValueTask<bool> ExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_users.Exists(u => u.Email == email));
    }

    public ValueTask<bool> ExistsAsync(NationalCode nationalCode, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_users.Exists(u => u.NationalCode == nationalCode));
    }
}