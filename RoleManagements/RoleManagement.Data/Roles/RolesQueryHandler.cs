using RoleManagement.Application.Roles;
using Tes.CQRS;

namespace RoleManagement.Data.Roles;

internal sealed class RolesQueryHandler : QueryHandler<RolesQuery, IEnumerable<RolesResponse>>
{
    public override async Task<IEnumerable<RolesResponse>> HandleAsync(RolesQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}