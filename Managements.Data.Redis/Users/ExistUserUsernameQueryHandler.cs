using Managements.Application.Users;
using Managements.Domain.Users.Exception;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

internal sealed class ExistUserUsernameQueryHandler : IQueryHandler<ExistUserUsernameQuery, Guid>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public ExistUserUsernameQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
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