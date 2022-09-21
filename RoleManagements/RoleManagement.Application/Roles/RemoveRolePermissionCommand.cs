using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record RemoveRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class RemoveRolePermissionCommandHandler : CommandHandler<RemoveRolePermissionCommand>
{
    public override async Task HandleAsync(RemoveRolePermissionCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}