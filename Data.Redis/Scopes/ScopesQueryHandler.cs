using Application.Scopes;
using Redis.OM;
using Redis.OM.Searching;
using Tes.CQRS;

namespace Data.Redis.Scopes;

internal sealed class ScopesQueryHandler : QueryHandler<ScopesQuery, IEnumerable<ScopesResponse>>
{
    private IRedisCollection<ScopeDataModel> _scopes;

    public ScopesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
    }

    public override async Task<IEnumerable<ScopesResponse>> HandleAsync(ScopesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _scopes = _scopes.Where(model => model.Id.Contains(message.Name));
        }

        var rolesDataModel = await _scopes.ToListAsync();

        return rolesDataModel.Select(model => new ScopesResponse(model.Id, model.Status));
    }
}