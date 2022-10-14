using Application.Users;
using Domain.Users.Exception;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Users;

internal sealed class UserIQueryHandler : IQueryHandler<UserQuery, UserResponse>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UserIQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async ValueTask<UserResponse> HandleAsync(UserQuery message, CancellationToken cancellationToken = default)
    {
        var user = await _users.FindByIdAsync(message.User.Value.ToString());
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return new UserResponse(user.Id, user.Usernames)
        {
            Status = user.Status,
            Roles = user.Roles,
            UserGroups = user.UserGroups
        };
    }
}