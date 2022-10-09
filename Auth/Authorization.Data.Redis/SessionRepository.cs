using Auth.Domain.Sessions;
using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;

namespace Authorization.Data.Redis;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly RedisCollection<SessionDataModel> _sessions;
    private readonly IMapper _mapper;

    public SessionRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _sessions = (RedisCollection<SessionDataModel>) provider.RedisCollection<SessionDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Session entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<SessionDataModel>(entity);
        await _sessions.InsertAsync(dataModel, TimeSpan.FromTicks(entity.ExpireDate.Ticks));
    }

    public async Task<Session?> FindAsync(Token id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(id.Value);
        return _mapper.Map<Session>(dataModel);
    }

    public async Task UpdateAsync(Session entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<SessionDataModel>(entity);
        await _sessions.UpdateAsync(dataModel);
    }

    public async Task DeleteAsync(Token id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _sessions.FindByIdAsync(id.Value);
        if (dataModel is null) return;
        await _sessions.DeleteAsync(dataModel);
    }

    public async ValueTask<bool> ExistsAsync(Token id, CancellationToken cancellationToken = default)
    {
        return await _sessions.AnyAsync(model => model.Id == id.Value);
    }

    public async ValueTask<IEnumerable<Session>> FindAsync(CancellationToken cancellationToken = default)
    {
        var dataModels = await _sessions.ToListAsync();
        return dataModels.Select(model => _mapper.Map<Session>(model));
    }
}