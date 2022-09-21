using RoleManagements.Domain;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;

internal sealed class CreateRoleCommandHandler : CommandHandler<CreateRoleCommand>
{
    public override async Task HandleAsync(CreateRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}