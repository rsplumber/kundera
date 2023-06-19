using Mediator;
using Queries.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Users;

public sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, bool>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public ExistUserUsernameQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<bool> Handle(ExistUserUsernameQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.Where(model => model.Usernames.Contains(query.Username)).FirstOrDefaultAsync();
        return user is not null;
    }
}