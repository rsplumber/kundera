using Core.Domains.Scopes.Exceptions;
using Managements.Data.Roles;
using Mediator;
using Queries.Scopes;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Scopes;

public sealed class ScopeRolesQueryHandler : IQueryHandler<ScopeRolesQuery, List<ScopeRolesResponse>>
{
    private readonly IRedisCollection<ScopeDataModel> _scopes;
    private readonly IRedisCollection<RoleDataModel> _roles;

    public ScopeRolesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>)provider.RedisCollection<ScopeDataModel>(false);
        _roles = (RedisCollection<RoleDataModel>)provider.RedisCollection<RoleDataModel>(false);
    }

    public async ValueTask<List<ScopeRolesResponse>> Handle(ScopeRolesQuery query, CancellationToken cancellationToken)
    {
        var scopeDataModel = await _scopes.FindByIdAsync(query.ScopeId.ToString());
        if (scopeDataModel is null)
        {
            throw new ScopeNotFoundException();
        }

        if (scopeDataModel.Roles is null)
        {
            return Array.Empty<ScopeRolesResponse>().ToList();
        }

        var rolesDataModel = await _roles.FindByIdsAsync(scopeDataModel.Roles.Select(guid => guid.ToString()));

        return rolesDataModel.Values.Select(model => new ScopeRolesResponse(model!.Id, model.Name)).ToList();
    }
}