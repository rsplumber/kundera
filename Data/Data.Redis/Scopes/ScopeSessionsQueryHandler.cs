﻿using Data.Abstractions.Scopes;
using Data.Auth.Sessions;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Scopes;

public sealed class ScopeSessionsQueryHandler : IQueryHandler<ScopeSessionsQuery, List<ScopeSessionResponse>>
{
    private readonly IRedisCollection<SessionDataModel> _sessions;

    public ScopeSessionsQueryHandler(RedisConnectionProvider provider)
    {
        _sessions = (RedisCollection<SessionDataModel>)provider.RedisCollection<SessionDataModel>(false);
    }

    public async ValueTask<List<ScopeSessionResponse>> Handle(ScopeSessionsQuery query, CancellationToken cancellationToken)
    {
        var sessions = await _sessions.Where(model => model.ScopeId == query.ScopeId).ToListAsync();

        return sessions.Select(model => new ScopeSessionResponse
        {
            Id = model.RefreshToken,
            UserId = model.UserId,
            ScopeId = model.ScopeId,
        }).ToList();
    }
}