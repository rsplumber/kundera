using AutoMapper;
using Kite.Events;
using Managements.Domain;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Scopes;

internal class ScopeRepository : IScopeRepository
{
    private readonly IEventBus _eventBus;
    private readonly RedisCollection<ScopeDataModel> _scopes;
    private readonly IMapper _mapper;


    public ScopeRepository(RedisConnectionProvider provider, IMapper mapper, IEventBus eventBus)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var scope = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.InsertAsync(scope);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Scope?> FindAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        var scopeDataModel = await _scopes.FindByIdAsync(id.ToString());

        return _mapper.Map<Scope>(scopeDataModel);
    }

    public async Task<bool> ExistsAsync(Name name, CancellationToken cancellationToken = default)
    {
        return await _scopes.AnyAsync(model => model.Name == name.Value);
    }

    public async Task<Scope?> FindAsync(Name name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _scopes.FirstOrDefaultAsync(model => model.Name == name.Value);

        return _mapper.Map<Scope>(dataModel);
    }

    public async Task<Scope?> FindAsync(ScopeSecret secret, CancellationToken cancellationToken = default)
    {
        var dataModel = await _scopes.FirstOrDefaultAsync(model => model.Secret == secret.Value);

        return _mapper.Map<Scope>(dataModel);
    }

    public async Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var scope = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.UpdateAsync(scope);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}