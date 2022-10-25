using Kite.CQRS;
using Managements.Application.Scopes;
using Managements.Data.Roles;
using Managements.Domain.Scopes.Exceptions;
using Redis.OM;
using Redis.OM.Searching;

namespace Managements.Data.Scopes;

internal sealed class ScopeRolesQueryHandler : IQueryHandler<ScopeRolesQuery, IEnumerable<ScopeRolesResponse>>
{
    private readonly IRedisCollection<ScopeDataModel> _scopes;
    private readonly IRedisCollection<RoleDataModel> _roles;

    public ScopeRolesQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
        _roles = (RedisCollection<RoleDataModel>) provider.RedisCollection<RoleDataModel>();
    }

    public async Task<IEnumerable<ScopeRolesResponse>> HandleAsync(ScopeRolesQuery message, CancellationToken cancellationToken = default)
    {
        var scopeDataModel = await _scopes.FindByIdAsync(message.Id.ToString());
        if (scopeDataModel is null)
        {
            throw new ScopeNotFoundException();
        }

        var rolesDataModel = await _roles.FindByIdsAsync(scopeDataModel.Roles.Select(guid => guid.ToString()));

        return rolesDataModel.Values.Select(model => new ScopeRolesResponse(model!.Id, model.Name));
    }
}