using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRoleMetaCommand(RoleId Role, IDictionary<string, string> Meta) : Command;

internal sealed class AddRoleMetaCommandHandler : CommandHandler<AddRoleMetaCommand>
{
    public override async Task HandleAsync(AddRoleMetaCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}