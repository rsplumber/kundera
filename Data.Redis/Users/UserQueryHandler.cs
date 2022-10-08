using Application.Users;
using Domain.Users.Exception;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Users;

internal sealed class UserQueryHandler : QueryHandler<UserQuery, UserResponse>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UserQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public override async Task<UserResponse> HandleAsync(UserQuery message, CancellationToken cancellationToken = default)
    {
        var user = await _users.FindByIdAsync(message.User.Value.ToString());
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return new UserResponse(user.Id, user.Usernames)
        {
            Roles = user.Roles,
            UserGroups = user.UserGroups
        };
    }
}