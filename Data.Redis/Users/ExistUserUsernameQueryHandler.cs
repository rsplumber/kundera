using Core.Domains.Users.Exception;
using Mediator;
using Queries.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

public sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, Guid>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public ExistUserUsernameQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<Guid> Handle(ExistUserUsernameQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.Where(model => model.Usernames.Contains(query.Username)).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user.Id;
    }
}