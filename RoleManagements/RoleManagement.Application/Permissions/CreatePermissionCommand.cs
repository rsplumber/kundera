using RoleManagements.Domain;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record CreatePermissionCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreatePermissionCommandHandler : CommandHandler<CreatePermissionCommand>
{
    public override async Task HandleAsync(CreatePermissionCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}