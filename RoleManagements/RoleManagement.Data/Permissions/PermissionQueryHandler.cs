using RoleManagement.Application.Permissions;
using Tes.CQRS;

namespace RoleManagement.Data.Permissions;

internal sealed class PermissionQueryHandler : QueryHandler<PermissionQuery,PermissionResponse>
{
    public override async Task<PermissionResponse> HandleAsync(PermissionQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}