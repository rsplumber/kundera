using RoleManagement.Application.Roles;
using Tes.CQRS;

namespace RoleManagement.Data.Roles;

internal sealed class RoleQueryHandler : QueryHandler<RoleQuery, RoleResponse>
{
    public override async Task<RoleResponse> HandleAsync(RoleQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}