using Redis.OM;
using Redis.OM.Searching;
using RoleManagement.Application.Scopes;
using RoleManagements.Domain.Roles.Exceptions;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Scopes;

internal sealed class ScopeQueryHandler : QueryHandler<ScopeQuery, ScopeResponse>
{
    private readonly IRedisCollection<ScopeDataModel> _scopes;

    public ScopeQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
    }

    public override async Task<ScopeResponse> HandleAsync(ScopeQuery message, CancellationToken cancellationToken = default)
    {
        var scope = await _scopes.FindByIdAsync(message.Scope.Value);
        if (scope is null)
        {
            throw new RoleNotFoundException();
        }

        return new ScopeResponse(scope.Id, scope.Status)
        {
            Roles = scope.Roles,
            Services = scope.Services
        };
    }
}