using RoleManagement.Application.Scopes;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Scopes;

internal sealed class ScopeQueryHandler : QueryHandler<ScopeQuery, ScopeResponse>
{
    public override async Task<ScopeResponse> HandleAsync(ScopeQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}