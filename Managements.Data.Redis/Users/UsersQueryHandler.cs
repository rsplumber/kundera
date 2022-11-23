using Managements.Application.Users;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Users;

internal sealed class UsersQueryHandler : IQueryHandler<UsersQuery, IEnumerable<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _users = (RedisCollection<UserDataModel>) provider.RedisCollection<UserDataModel>();
    }

    public async ValueTask<IEnumerable<UsersResponse>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _users.ToListAsync();
        return users.Select(model => new UsersResponse(model.Id, model.Usernames))
            .ToList();
    }
}