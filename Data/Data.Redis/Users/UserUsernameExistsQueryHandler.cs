using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Users;

public sealed class UserUsernameExistsQueryHandler : IQueryHandler<UserUsernameExistsQuery, UserUsernameExistsResponse>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UserUsernameExistsQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<UserUsernameExistsResponse> Handle(UserUsernameExistsQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.Where(model => model.Usernames.Contains(query.Username)).FirstOrDefaultAsync();
        if (user is null) throw new UserNotFoundException();
        return new UserUsernameExistsResponse
        {
            Id = user.Id
        };
    }
}