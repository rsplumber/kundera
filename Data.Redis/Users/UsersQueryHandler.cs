using Mediator;
using Queries.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

public sealed class UsersQueryHandler : IQueryHandler<UsersQuery, List<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<List<UsersResponse>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _users.ToListAsync();
        return users.Select(model => new UsersResponse(model.Id, model.Usernames))
            .ToList();
    }
}