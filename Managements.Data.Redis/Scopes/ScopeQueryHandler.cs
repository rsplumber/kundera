using Managements.Application.Scopes;
using Managements.Domain.Roles.Exceptions;
using Mediator;
using Redis.OM.Searching;

namespace Managements.Data.Scopes;

internal sealed class ScopeQueryHandler : IQueryHandler<ScopeQuery, ScopeResponse>
{
    private readonly IRedisCollection<ScopeDataModel> _scopes;

    public ScopeQueryHandler(RedisConnectionManagementsProviderWrapper provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>) provider.RedisCollection<ScopeDataModel>();
    }

    public async ValueTask<ScopeResponse> Handle(ScopeQuery query, CancellationToken cancellationToken)
    {
        var scope = await _scopes.FindByIdAsync(query.Scope.ToString());
        if (scope is null)
        {
            throw new RoleNotFoundException();
        }

        return new ScopeResponse(scope.Id, scope.Name, scope.Secret, scope.Status)
        {
            Roles = scope.Roles,
            Services = scope.Services
        };
    }
}