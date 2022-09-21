using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record DeActivateServiceCommand(ServiceId Service) : Command;

internal sealed class DeActivateServiceCommandHandler : CommandHandler<DeActivateServiceCommand>
{
    public override async Task HandleAsync(DeActivateServiceCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}