using RoleManagement.Application.Permissions;
using Tes.CQRS;

namespace RoleManagement.Data.Permissions;

internal sealed class PermissionsQueryHandler : QueryHandler<PermissionsQuery, IEnumerable<PermissionsResponse>>
{
    public override async Task<IEnumerable<PermissionsResponse>> HandleAsync(PermissionsQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}