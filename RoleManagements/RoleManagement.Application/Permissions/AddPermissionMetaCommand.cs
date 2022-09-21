using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record AddPermissionMetaCommand(PermissionId Permission, IDictionary<string, string> Meta) : Command;

internal sealed class AddPermissionMetaCommandHandler : CommandHandler<AddPermissionMetaCommand>
{
    public override async Task HandleAsync(AddPermissionMetaCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}