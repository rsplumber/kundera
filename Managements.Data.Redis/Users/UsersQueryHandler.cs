using Kite.CQRS;
using Managements.Application.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Users;

internal sealed class UsersQueryHandler : IQueryHandler<UsersQuery, IEnumerable<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async ValueTask<IEnumerable<UsersResponse>> HandleAsync(UsersQuery message, CancellationToken cancellationToken = default)
    {
        var users = await _users.ToListAsync();

        return users.Select(model => new UsersResponse(model.Id, model.Usernames))
                    .ToList();
    }
}