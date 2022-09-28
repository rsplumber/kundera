using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;
using Users.Application.Users;

namespace Users.Data.Redis.Users;

internal sealed class UsersQueryHandler : QueryHandler<UsersQuery, IEnumerable<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public override async Task<IEnumerable<UsersResponse>> HandleAsync(UsersQuery message, CancellationToken cancellationToken = default)
    {
        return await _users.Select(model => new UsersResponse(model.Id, model.Usernames)).ToListAsync();
    }
}