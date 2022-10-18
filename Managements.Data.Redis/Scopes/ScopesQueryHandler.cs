using Kite.CQRS;
using Managements.Application.Scopes;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Redis.Scopes;

internal sealed class ScopesQueryHandler : IQueryHandler<ScopesQuery, IEnumerable<ScopesResponse>>
{
    private IRedisCollection<ScopeDataModel> _scopes;

    public ScopesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
    }

    public async Task<IEnumerable<ScopesResponse>> HandleAsync(ScopesQuery message, CancellationToken cancellationToken = default)
    {
        if (message.Name is not null)
        {
            _scopes = _scopes.Where(model => model.Name.Contains(message.Name));
        }

        var rolesDataModel = await _scopes.ToListAsync();

        return rolesDataModel.Select(model => new ScopesResponse(model.Id, model.Name, model.Status));
    }
}