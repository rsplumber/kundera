using Application.Users;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Users;

internal sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, bool>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public ExistUserUsernameQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async ValueTask<bool> HandleAsync(ExistUserUsernameQuery message, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(model => model.Usernames.Contains(message.Username));
    }
}