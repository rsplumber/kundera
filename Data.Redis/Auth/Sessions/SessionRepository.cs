using AutoMapper;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Users.Types;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Auth.Sessions;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly RedisCollection<SessionDataModel> _sessions;
    private readonly IMapper _mapper;

    public SessionRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _sessions = (RedisCollection<SessionDataModel>)provider.RedisCollection<SessionDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Session entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<SessionDataModel>(entity);
        await _sessions.InsertAsync(dataModel);
    }

    public async Task<Session?> FindAsync(Token id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(id.Value);
        return _mapper.Map<Session>(dataModel);
    }

    public async Task<Session?> FindAsync(CredentialId credentialId, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.Where(model => model.CredentialId == credentialId.Value).FirstOrDefaultAsync();
        return _mapper.Map<Session>(dataModel);
    }

    public async Task UpdateAsync(Session entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<SessionDataModel>(entity);
        await _sessions.InsertAsync(dataModel);
    }

    public async Task DeleteAsync(Token id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(id.Value);
        if (dataModel is null) return;
        await _sessions.DeleteAsync(dataModel);
    }

    public async Task<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default)
    {
        var dataModels = await _sessions.ToListAsync();
        return dataModels.Select(model => _mapper.Map<Session>(model));
    }

    public async Task<IEnumerable<Session>> FindAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var dataModels = await _sessions.Where(model => model.UserId == userId.Value).ToListAsync();
        return dataModels.Select(model => _mapper.Map<Session>(model));
    }

    public async Task DeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _sessions.ToListAsync();
        var expiredSessions = sessions.Where(model => DateTime.UtcNow >= model.ExpiresAt.ToUniversalTime());
        foreach (var credential in expiredSessions)
        {
            await _sessions.DeleteAsync(credential);
        }
    }
}