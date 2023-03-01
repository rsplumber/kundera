using Application.Scopes;
using Core.Domains.Roles.Exceptions;
using Mediator;
using Redis.OM;
using Redis.OM.Searching;

namespace Data.Scopes;

public sealed class ScopeQueryHandler : IQueryHandler<ScopeQuery, ScopeResponse>
{
    private readonly IRedisCollection<ScopeDataModel> _scopes;

    public ScopeQueryHandler(RedisConnectionProvider provider)
    {
        _scopes = (RedisCollection<ScopeDataModel>)provider.RedisCollection<ScopeDataModel>(false);
    }

    public async ValueTask<ScopeResponse> Handle(ScopeQuery query, CancellationToken cancellationToken)
    {
        var scope = await _scopes.FindByIdAsync(query.ScopeId.ToString());
        if (scope is null)
        {
            throw new RoleNotFoundException();
        }

        return new ScopeResponse
        {
            Id = scope.Id,
            Name = scope.Name,
            Secret = scope.Secret,
            Status = scope.Status,
            Roles = scope.Roles,
            Services = scope.Services
        };
    }
}