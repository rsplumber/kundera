using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRolePermissionCommand(RoleId Role, params PermissionId[] Permissions) : Command;

internal sealed class AddRolePermissionCommandHandler : CommandHandler<AddRolePermissionCommand>
{
    public override async Task HandleAsync(AddRolePermissionCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}