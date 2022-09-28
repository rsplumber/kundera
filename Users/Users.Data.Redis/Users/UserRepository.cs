using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;
using Users.Domain.Users;

namespace Users.Data.Redis.Users;

internal class UserRepository : IUserRepository
{
    private readonly RedisCollection<UserDataModel> _users;
    private readonly IMapper _mapper;

    public UserRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _mapper = mapper;
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        var userDataModel = _mapper.Map<UserDataModel>(entity);
        await _users.InsertAsync(userDataModel);
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var userDataModel = await _users.FindByIdAsync(id.Value.ToString());
        return _mapper.Map<User>(userDataModel);
    }

    public async ValueTask<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(model => model.Usernames.Any(u => u == username));
    }

    public async Task<User> FindAsync(Username username, CancellationToken cancellationToken = default)
    {
        var userDataModel = await _users.FirstOrDefaultAsync(model => model.Usernames.Any(u => u == username));
        return _mapper.Map<User>(userDataModel);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var userDataModel = _mapper.Map<UserDataModel>(entity);
        await _users.InsertAsync(userDataModel);
    }
}