using Kite.CQRS;
using Managements.Application.Users;
using Managements.Domain.Users.Exception;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

internal sealed class UserQueryHandler : IQueryHandler<UserQuery, UserResponse>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UserQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async Task<UserResponse> HandleAsync(UserQuery message, CancellationToken cancellationToken = default)
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
            Groups = user.Groups
        };
    }
}