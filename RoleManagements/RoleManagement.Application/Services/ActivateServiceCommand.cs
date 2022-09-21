using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record ActivateServiceCommand(ServiceId Service) : Command;

internal sealed class ActivateServiceCommandHandler : CommandHandler<ActivateServiceCommand>
{
    public override async Task HandleAsync(ActivateServiceCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}