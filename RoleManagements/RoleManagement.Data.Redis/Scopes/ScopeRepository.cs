using AutoMapper;
using Redis.OM;
using Redis.OM.Searching;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Types;

namespace RoleManagement.Data.Redis.Scopes;

internal class ScopeRepository : IScopeRepository
{
    private readonly RedisCollection<ScopeDataModel> _scopes;
    private readonly IMapper _mapper;


    public ScopeRepository(RedisConnectionProvider provider, IMapper mapper)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
        _mapper = mapper;
    }

    public async Task AddAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var scope = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.InsertAsync(scope);
    }

    public async Task<Scope?> FindAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        var scopeDataModel = await _scopes.FindByIdAsync(id.Value);
        return _mapper.Map<Scope>(scopeDataModel);
    }

    public async ValueTask<bool> ExistsAsync(ScopeId id, CancellationToken cancellationToken = default)
    {
        return await _scopes.AnyAsync(model => model.Id == id.Value);
    }

    public async Task UpdateAsync(Scope entity, CancellationToken cancellationToken = default)
    {
        var scope = _mapper.Map<ScopeDataModel>(entity);
        await _scopes.UpdateAsync(scope);
    }
}