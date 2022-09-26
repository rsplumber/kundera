using RoleManagement.Application.Scopes;
using Tes.CQRS;

namespace RoleManagement.Data.Redis.Scopes;

internal sealed class ScopesQueryHandler : QueryHandler<ScopesQuery, IEnumerable<ScopesResponse>>
{
    public override async Task<IEnumerable<ScopesResponse>> HandleAsync(ScopesQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}