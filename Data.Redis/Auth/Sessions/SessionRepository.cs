﻿using AutoMapper;
using Core.Domains.Auth.Sessions;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Auth.Sessions;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<SessionDataModel> _sessions;
    private readonly IMapper _mapper;

    public SessionRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _sessions = (RedisCollection<SessionDataModel>)provider.RedisCollection<SessionDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Session session, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<SessionDataModel>(session);
        await _sessions.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(session);
    }

    public async Task<Session?> FindAsync(string token, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(token);
        return _mapper.Map<Session>(dataModel);
    }

    public async Task<Session?> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.Where(model => model.RefreshToken == token).FirstOrDefaultAsync();
        return _mapper.Map<Session>(dataModel);
    }

    public async Task<IEnumerable<Session>> FindByCredentialIdAsync(Guid credentialId, CancellationToken cancellationToken = default)
    {
        var dataModels = await _sessions.Where(model => model.CredentialId == credentialId).ToListAsync();
        return dataModels.Select(model => _mapper.Map<Session>(model));
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(token);
        if (dataModel is null) return;
        await _sessions.DeleteAsync(dataModel);
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _sessions.ToListAsync();
        var expiredSessions = sessions.Where(model => DateTime.UtcNow >= model.ExpirationDateUtc.ToUniversalTime());
        foreach (var credential in expiredSessions)
        {
            await _sessions.DeleteAsync(credential);
        }
    }
}