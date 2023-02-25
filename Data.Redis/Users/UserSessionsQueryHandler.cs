﻿using Managements.Data.Auth.Sessions;
using Mediator;
using Queries.Users;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Users;

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
            Id = model.Id,
            ScopeId = model.ScopeId,
            ExpirationDateUtc = model.ExpirationDateUtc,
            LastIpAddress = model.LastIpAddress,
            LastUsageDateUtc = model.LastUsageDateUtc
        }).ToList();
    }
}