using Core.Users.Exception;
using Data.Abstractions.Users;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Users;

public sealed class UserQueryHandler : IQueryHandler<UserQuery, UserResponse>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UserQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<UserResponse> Handle(UserQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.FindByIdAsync(query.UserId.ToString());
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return new UserResponse
        {
            Id = user.Id,
            Usernames = user.Usernames,
            Status = user.Status,
            Roles = user.Roles,
            Groups = user.Groups
        };
    }
}