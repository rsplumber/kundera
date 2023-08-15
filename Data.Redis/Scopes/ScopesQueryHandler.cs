using Data.Abstractions;
using Data.Abstractions.Scopes;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Scopes;

public sealed class ScopesQueryHandler : IQueryHandler<ScopesQuery, PageableResponse<ScopesResponse>>
{
    private IRedisCollection<ScopeDataModel> _scopes;

    public ScopesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>)provider.RedisCollection<ScopeDataModel>(false);
    }

    public async ValueTask<PageableResponse<ScopesResponse>> Handle(ScopesQuery query, CancellationToken cancellationToken)
    {
        if (query.Name is not null)
        {
            _scopes = _scopes.Where(model => model.Name.Contains(query.Name));
        }

        var scopesQuery = await _scopes.Page(query).ToListAsync();
        var scopes = scopesQuery.Select(model => new ScopesResponse(model.Id, model.Name, model.Status)).ToList();

        int counts;
        if (query.Name is not null)
        {
            counts = await _scopes.Where(model => model.Name.Contains(query.Name)).CountAsync();
        }
        else
        {
            counts = await _scopes.CountAsync();
        }

        return new PageableResponse<ScopesResponse>
        {
            Data = scopes,
            TotalItems = counts,
            TotalPages = counts / query.Size
        };
    }
}