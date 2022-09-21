using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record RemovePermissionMetaCommand(PermissionId Permission, params string[] MetaKeys) : Command;

internal sealed class RemovePermissionMetaCommandHandler : CommandHandler<RemovePermissionMetaCommand>
{
    public override async Task HandleAsync(RemovePermissionMetaCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}