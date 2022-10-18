using Kite.CQRS;
using Managements.Application.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Users;

internal sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, bool>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public ExistUserUsernameQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async Task<bool> HandleAsync(ExistUserUsernameQuery message, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(model => model.Usernames.Contains(message.Username));
    }
}