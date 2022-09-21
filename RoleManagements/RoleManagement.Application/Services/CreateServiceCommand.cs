using RoleManagements.Domain;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;

internal sealed class CreateServiceCommandHandler : CommandHandler<CreateServiceCommand>
{
    public override async Task HandleAsync(CreateServiceCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}