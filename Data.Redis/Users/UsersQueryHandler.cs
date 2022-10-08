using Application.Users;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Users;

internal sealed class UsersQueryHandler : QueryHandler<UsersQuery, IEnumerable<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public override async Task<IEnumerable<UsersResponse>> HandleAsync(UsersQuery message, CancellationToken cancellationToken = default)
    {
        var users = await _users.ToListAsync();
        return users.Select(model => new UsersResponse(model.Id, model.Usernames)).ToList();
    }
}