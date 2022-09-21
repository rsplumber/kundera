using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record RemoveRoleMetaCommand(RoleId Role, params string[] MetaKeys) : Command;

internal sealed class RemoveRoleMetaCommandHandler : CommandHandler<RemoveRoleMetaCommand>
{
    public override async Task HandleAsync(RemoveRoleMetaCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}