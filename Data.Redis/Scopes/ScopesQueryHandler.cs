using Application.Scopes;
using Kite.CQRS;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Redis.Scopes;

internal sealed class ScopesIQueryHandler : IQueryHandler<ScopesQuery, IEnumerable<ScopesResponse>>
{
    private IRedisCollection<ScopeDataModel> _scopes;

    public ScopesIQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
    }

    public async ValueTask<IEnumerable<ScopesResponse>> HandleAsync(ScopesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _scopes = _scopes.Where(model => model.Id.Contains(message.Name));
        }

        var rolesDataModel = await _scopes.ToListAsync();

        return rolesDataModel.Select(model => new ScopesResponse(model.Id, model.Status));
    }
}