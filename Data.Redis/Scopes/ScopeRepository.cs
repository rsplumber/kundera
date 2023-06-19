using AutoMapper;
using Core.Scopes;
using DotNetCore.CAP;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Scopes;

internal class ScopeRepository : IScopeRepository
{
    private readonly ICapPublisher _eventBus;
    private readonly RedisCollection<ScopeDataModel> _scopes;
    private readonly IMapper _mapper;


    public ScopeRepository(RedisConnectionProvider provider, IMapper mapper, ICapPublisher eventBus)
    {
        _scopes = (RedisCollection<ScopeDataModel>)provider.RedisCollection<ScopeDataModel>();
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var dataModel = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.InsertAsync(dataModel);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public async Task<Scope?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _scopes.FindByIdAsync(id.ToString());
        return _mapper.Map<Scope>(dataModel);
    }

    public async Task<Scope?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var dataModel = await _scopes.FirstOrDefaultAsync(model => model.Name == name);
        return _mapper.Map<Scope>(dataModel);
    }

    public async Task<Scope?> FindBySecretAsync(string secret, CancellationToken cancellationToken = default)
    {
        var dataModel = await _scopes.FirstOrDefaultAsync(model => model.Secret == secret);
        return _mapper.Map<Scope>(dataModel);
    }

    public async Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var scope = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.InsertAsync(scope);
        await _eventBus.DispatchDomainEventsAsync(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}