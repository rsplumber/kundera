using Mediator;
using Queries.Scopes;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Scopes;

internal sealed class ScopesQueryHandler : IQueryHandler<ScopesQuery, List<ScopesResponse>>
{
    private IRedisCollection<ScopeDataModel> _scopes;

    public ScopesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>)provider.RedisCollection<ScopeDataModel>(false);
    }

    public async ValueTask<List<ScopesResponse>> Handle(ScopesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _scopes = _scopes.Where(model => model.Name.Contains(query.Name));
        }

        var rolesDataModel = await _scopes.ToListAsync();

        return rolesDataModel.Select(model => new ScopesResponse(model.Id, model.Name, model.Status)).ToList();
    }
}