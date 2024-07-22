using Data.Abstractions;
using Data.Abstractions.Users;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Users;

public sealed class UsersQueryHandler : IQueryHandler<UsersQuery, PageableResponse<UsersResponse>>
{
    private readonly IRedisCollection<UserDataModel> _users;

    public UsersQueryHandler(RedisConnectionProvider provider)
    {
        _users = (RedisCollection<UserDataModel>)provider.RedisCollection<UserDataModel>(false);
    }

    public async ValueTask<PageableResponse<UsersResponse>> Handle(UsersQuery query, CancellationToken cancellationToken)
    {
        var usersQuery = await _users.Page(query).ToListAsync();
        var users = usersQuery.Select(model => new UsersResponse(model.Id))
            .ToList();
        var counts = await _users.CountAsync();

        return new PageableResponse<UsersResponse>
        {
            Data = users,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}