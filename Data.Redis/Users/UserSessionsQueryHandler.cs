using Application.Users;
using Data.Auth.Sessions;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Users;

public sealed class UserSessionsQueryHandler : IQueryHandler<UserSessionsQuery, List<UserSessionResponse>>
{
    private readonly IRedisCollection<SessionDataModel> _sessions;

    public UserSessionsQueryHandler(RedisConnectionProvider provider)
    {
        _sessions = (RedisCollection<SessionDataModel>)provider.RedisCollection<SessionDataModel>(false);
    }

    public async ValueTask<List<UserSessionResponse>> Handle(UserSessionsQuery query, CancellationToken cancellationToken)
    {
        var sessions = await _sessions.Where(model => model.UserId == query.UserId).ToListAsync();

        return sessions.Select(model => new UserSessionResponse
        {
            Id = model.RefreshToken,
            ScopeId = model.ScopeId,
            ExpirationDateUtc = model.ExpirationDateUtc,
            LastIpAddress = model.LastIpAddress,
            LastUsageDateUtc = model.LastUsageDateUtc
        }).ToList();
    }
}